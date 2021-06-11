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
using System.Reflection;

namespace Default
{
    [CreateAssetMenu(menuName = MenuPath + "Asset")]
    public class WebSocketServerCore : ServersCore.Module
    {
        new public const string MenuPath = ServersCore.Module.MenuPath + "Web Socket/";

        [SerializeField]
        protected int port = 5460;
        public int Port { get { return port; } }

        [SerializeField]
        protected int capacity = 4;
        public int Capacity { get { return capacity; } }

        public WebSocketServer Server { get; protected set; }

        public override bool Active
        {
            get
            {
                if (Server == null) return false;

                return Server.IsListening;
            }
        }

        public WebServerCore WebServer { get { return Core.Servers.WebServer; } }

        public DNSCore DNSServer { get { return Core.Servers.DNS; } }

        public override void Configure()
        {
            base.Configure();

            port = OptionsOverride.Get("Web Socket Port", port);

            Application.runInBackground = true;
        }

        public override void Start()
        {
            try
            {
                Server = new WebSocketServer(Address, port);

                Server.KeepClean = true;

                Server.AddWebSocketService<WSSBehaviour>("/");

                Server.Log.Level = LogLevel.Error;
                Server.Log.Output = (data, s) => Debug.LogError(data.Message);

                var listener = typeof(WebSocketServer).GetField("_listener", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Server) as TcpListener;
                listener.Server.NoDelay = true;

                Server.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Error when initiating server, message: " + e.ToString());
                throw;
            }
        }

        public delegate void ContextOperationDelegate(WSSBehaviour behaviour);
        public event ContextOperationDelegate ConnectionEvent;
        internal void OnConnection(WSSBehaviour behaviour)
        {
            ConnectionEvent?.Invoke(behaviour);
        }

        public delegate void MessageDelegate(WSSBehaviour behaviour, MessageEventArgs args);
        public event MessageDelegate MessageEvent;
        internal void OnMessage(WSSBehaviour behaviour, MessageEventArgs args)
        {
            MessageEvent?.Invoke(behaviour, args);
        }

        public delegate void DisconnectOperationDelegate(WSSBehaviour behaviour, CloseEventArgs args);
        public event DisconnectOperationDelegate DisconnectionEvent;
        internal void OnDisconnection(WSSBehaviour behaviour, CloseEventArgs args)
        {
            DisconnectionEvent?.Invoke(behaviour, args);
        }

        public override void Stop()
        {
            if (!Active) return;

            Server.Stop();
            Server = null;
        }

        public abstract partial class Module : Core.Module
        {
            new public const string MenuPath = WebSocketServerCore.MenuPath + "Modules/";

            public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }
        }
    }

    public class WSSBehaviour : WebSocketBehavior
    {
        public Core Core { get { return Core.Asset; } }
        public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }

        protected override void OnOpen()
        {
            base.OnOpen();

            UnityThreadDispatcher.Add(OnOpen_UNITYSAFE);
        }
        void OnOpen_UNITYSAFE()
        {
            WebSocketServer.OnConnection(this);
        }

        protected override void OnMessage(MessageEventArgs args)
        {
            base.OnMessage(args);

            UnityThreadDispatcher.Add(() => OnMessage_UNITY_SAFE(args));
        }
        void OnMessage_UNITY_SAFE(MessageEventArgs args)
        {
            WebSocketServer.OnMessage(this, args);
        }

        protected override void OnClose(CloseEventArgs args)
        {
            base.OnClose(args);

            UnityThreadDispatcher.Add(() => OnClose_UNITY_SAFE(args));
        }
        void OnClose_UNITY_SAFE(CloseEventArgs args)
        {
            WebSocketServer.OnDisconnection(this, args);
        }
    }
}