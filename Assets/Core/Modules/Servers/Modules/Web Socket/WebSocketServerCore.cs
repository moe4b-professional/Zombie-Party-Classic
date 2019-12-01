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
    public class WebSocketServerCore : ServersCore.Module
	{
        new public const string MenuPath = ServersCore.Module.MenuPath + "Web Socket/";

        [SerializeField]
        protected int port = 5460;
        public int Port { get { return port; } }

        [SerializeField]
        protected int size = 4;
        public int Size { get { return size; } }

        public WebSocketServer Server { get; protected set; }

        public override bool Active
        {
            get
            {
                if (Server == null) return false;

                return Server.IsListening;
            }
        }

        public InternalBehavior Behavior { get; protected set; }
        void InitBehaviour(InternalBehavior behaviour)
        {
            this.Behavior = behaviour;
        }
        public class InternalBehavior : WebSocketBehavior
        {
            public Core Core { get { return Core.Asset; } }
            public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }
            public ClientsManagerCore Clients { get { return WebSocketServer.Clients; } }

            protected override void OnOpen()
            {
                base.OnOpen();

                UnityThreadDispatcher.Add(()=>OnOpen_UNITYSAFE(Context));
            }
            void OnOpen_UNITYSAFE(WebSocketContext context)
            {
                WebSocketServer.OnConnection(Context);
            }

            protected override void OnMessage(MessageEventArgs args)
            {
                base.OnMessage(args);

                UnityThreadDispatcher.Add(()=>OnMessage_UNITY_SAFE(Context, args));
            }
            void OnMessage_UNITY_SAFE(WebSocketContext context, MessageEventArgs args)
            {
                WebSocketServer.OnMessage(Context, args);
            }

            protected override void OnClose(CloseEventArgs args)
            {
                base.OnClose(args);

                UnityThreadDispatcher.Add(()=>OnClose_UNITY_SAFE(Context, args));
            }
            void OnClose_UNITY_SAFE(WebSocketContext context, CloseEventArgs args)
            {
                WebSocketServer.OnDisconnection(Context, args);
            }

            public InternalBehavior()
            {
                WebSocketServer.InitBehaviour(this);
            }
        }

        [SerializeField]
        protected ClientsManagerCore clients;
        public ClientsManagerCore Clients { get { return clients; } }

        public WebServerCore WebServer { get { return Core.Servers.WebServer; } }

        public DNSCore DNSServer { get { return Core.Servers.DNS; } }

        public override void Configure()
        {
            base.Configure();

            port = OptionsOverride.Get("Web Socket Port", port);

            Application.runInBackground = true;

            NetworkMessage.Configure();

            clients.Configure();
        }

        public override void Init()
        {
            base.Init();

            clients.Init();
        }

        public override void Start()
        {
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

        public override void Stop()
        {
            if (!Active) return;

            Server.Stop();

            Server = null;
            Behavior = null;
        }

        public abstract partial class Module : Core.Module
        {
            new public const string MenuPath = WebSocketServerCore.MenuPath + "Modules/";

            public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }

            public InternalBehavior Behaviour { get { return WebSocketServer.Behavior; } }
        }
    }
}