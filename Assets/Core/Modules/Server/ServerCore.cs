using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using System.Runtime.Serialization;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using System.Threading;

using System.Text.RegularExpressions;

using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game
{
    [CreateAssetMenu(menuName = MenuPath + "Asset")]
    public class ServerCore : Core.Module
	{
        new public const string MenuPath = Core.Module.MenuPath + "Server/";

        public bool Active
        {
            get
            {
                if (Server == null) return false;

                return Server.IsListening;
            }
        }

        [SerializeField]
        protected int port = 8080;
        public int Port { get { return port; } }

        [SerializeField]
        protected int size = 4;
        public int Size { get { return size; } }

        public IPAddress Address { get; protected set; }
        protected virtual void InitAddress()
        {
            try
            {
                Address = OptionsOverride.Get("Internal IP Address", IPAddress.Parse, IPAddress.Any);
            }
            catch (Exception)
            {
                Debug.LogError("Error when getting Internal IP Address, Using any IP instead");

                Address = IPAddress.Any;
            }
        }

        public WebSocketServer Server { get; protected set; }

        public InternalBehavior Behavior { get; protected set; }
        void InitBehaviour(InternalBehavior behaviour)
        {
            this.Behavior = behaviour;
        }
        public class InternalBehavior : WebSocketBehavior
        {
            public ServerCore Server { get { return Core.Asset.Server; } }
            public ClientsManagerCore Clients { get { return Server.Clients; } }

            protected override void OnOpen()
            {
                base.OnOpen();

                UnityThreadDispatcher.Add(()=>OnOpen_UNITYSAFE(Context));
            }
            void OnOpen_UNITYSAFE(WebSocketContext context)
            {
                Server.OnConnection(Context);
            }

            protected override void OnMessage(MessageEventArgs args)
            {
                base.OnMessage(args);

                UnityThreadDispatcher.Add(()=>OnMessage_UNITY_SAFE(Context, args));
            }
            void OnMessage_UNITY_SAFE(WebSocketContext context, MessageEventArgs args)
            {
                Server.OnMessage(Context, args);
            }

            protected override void OnClose(CloseEventArgs args)
            {
                base.OnClose(args);

                UnityThreadDispatcher.Add(()=>OnClose_UNITY_SAFE(Context, args));
            }
            void OnClose_UNITY_SAFE(WebSocketContext context, CloseEventArgs args)
            {
                Server.OnDisconnection(Context, args);
            }

            public InternalBehavior()
            {
                Server.InitBehaviour(this);
            }
        }

        [SerializeField]
        protected ClientsManagerCore clients;
        public ClientsManagerCore Clients { get { return clients; } }

        public WebServerCore WebServer { get { return Core.WebServer; } }

        public override void Configure()
        {
            base.Configure();

            port = OptionsOverride.Get("server port", int.Parse, port);

            Application.runInBackground = true;

            NetworkMessage.Configure();

            clients.Configure();
        }

        public override void Init()
        {
            base.Init();

            clients.Init();
        }

        public virtual void Start()
        {
            InitAddress();

            try
            {
                Server = new WebSocketServer(Address, port);

                Server.KeepClean = true;

                Server.AddWebSocketService<InternalBehavior>("/");

                Server.Log.Level = LogLevel.Error;
                Server.Log.Output = (data, s) => { Debug.LogError(data.Message); };

                Server.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Error when initiating server, message: " + e.ToString());
                throw;
            }

            WebServer.Start();
        }

        public delegate void ContextOperationDelegate(WebSocketContext context);
        public event ContextOperationDelegate ConnectionEvent;
        void OnConnection(WebSocketContext context)
        {
            if (ConnectionEvent != null) ConnectionEvent(context);
        }

        public delegate void MessageDelegate(WebSocketContext context, MessageEventArgs args);
        public event MessageDelegate MessageEvent;
        void OnMessage(WebSocketContext context, MessageEventArgs args)
        {
            if (MessageEvent != null) MessageEvent(context, args);
        }

        public delegate void DisconnectOperationDelegate(WebSocketContext context, CloseEventArgs args);
        public event DisconnectOperationDelegate DisconnectionEvent;
        void OnDisconnection(WebSocketContext context, CloseEventArgs args)
        {
            if (DisconnectionEvent != null) DisconnectionEvent(context, args);
        }

        public virtual void Stop()
        {
            if (!Active) return;

            Server.Stop();

            Server = null;
            Behavior = null;
        }


        public abstract partial class Module : Core.Module
        {
            new public const string MenuPath = ServerCore.MenuPath + "Modules/";

            public ServerCore Server { get { return Core.Server; } }

            public InternalBehavior Behaviour { get { return Server.Behavior; } }
        }
    }
}