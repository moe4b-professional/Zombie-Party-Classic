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

        public string Address { get; protected set; }

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

            public delegate void ContextOperationDelegate(WebSocketContext context);


            protected override void OnOpen()
            {
                base.OnOpen();

                UnityThreadDispatcher.Add(OnOpen_UNITYSAFE(Context));
            }

            ContextOperationDelegate ConnectionAction;
            IEnumerator OnOpen_UNITYSAFE(WebSocketContext context)
            {
                if (ConnectionAction != null) ConnectionAction(Context);

                yield break;
            }


            protected override void OnMessage(MessageEventArgs e)
            {
                base.OnMessage(e);

                UnityThreadDispatcher.Add(OnMessage_UNITY_SAFE(e));
            }

            public delegate void MessageDelegate(WebSocketContext context, MessageEventArgs args);
            MessageDelegate MessageAction;
            IEnumerator OnMessage_UNITY_SAFE(MessageEventArgs e)
            {
                if (MessageAction != null) MessageAction(Context, e);

                yield break;
            }


            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);

                UnityThreadDispatcher.Add(OnClose_UNITY_SAFE(Context, e));
            }

            public delegate void DisconnectOperationDelegate(WebSocketContext context, CloseEventArgs args);
            DisconnectOperationDelegate DisconnectAction;
            IEnumerator OnClose_UNITY_SAFE(WebSocketContext context, CloseEventArgs e)
            {
                DisconnectAction(Context, e);

                yield break;
            }


            public InternalBehavior()
            {
                Server.InitBehaviour(this);

                ConnectionAction = Server.OnConnection;
                MessageAction = Server.OnMessage;
                DisconnectAction = Server.OnDisconnection;
            }
        }

        [SerializeField]
        protected ClientsManagerCore clients;
        public ClientsManagerCore Clients { get { return clients; } }

        public WebServerCore WebServer { get { return Core.WebServer; } }

        public override void Configure()
        {
            base.Configure();

            Application.runInBackground = true;

            NetworkMessage.Configure();

            clients.Configure();

            Core.SceneAccessor.ApplicationQuitEvent += OnApplicationQuit;
        }

        public override void Init()
        {
            base.Init();

            clients.Init();
        }

        public virtual void Start()
        {
            Address = GetLANIP();

            try
            {
                Server = new WebSocketServer(IPAddress.Any, port);

                Server.AddWebSocketService<InternalBehavior>("/");

                Server.Log.Level = LogLevel.Error;
                Server.Log.Output = (data, s) => { Debug.LogError(data.Message); };

                Server.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Error when starting server, message: " + e.Message);
            }

            WebServer.Start();
        }

        public event InternalBehavior.ContextOperationDelegate ConnectionEvent;
        void OnConnection(WebSocketContext context)
        {
            if (ConnectionEvent != null) ConnectionEvent(context);
        }

        public event InternalBehavior.MessageDelegate MessageEvent;
        void OnMessage(WebSocketContext context, MessageEventArgs args)
        {
            if (MessageEvent != null) MessageEvent(context, args);
        }

        public event InternalBehavior.DisconnectOperationDelegate DisconnectionEvent;
        void OnDisconnection(WebSocketContext context, CloseEventArgs args)
        {
            if (DisconnectionEvent != null) DisconnectionEvent(context, args);
        }

        public virtual void Stop()
        {
            if (Active)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Server.Stop(CloseStatusCode.Normal, "Session Ended");
#pragma warning restore CS0618 // Type or member is obsolete
            }

            if (WebServer.Active)
                WebServer.Stop();
        }
        void OnApplicationQuit()
        {
            Stop();
        }


        public abstract partial class Module : Core.Module
        {
            new public const string MenuPath = ServerCore.MenuPath + "Modules/";

            public ServerCore Server { get { return Core.Server; } }

            public InternalBehavior Behaviour { get { return Server.Behavior; } }
        }


        public const string LocalHost = "127.0.0.1";
        public static string GetLANIP()
        {
            var interfaceTypes = new NetworkInterfaceType[] { NetworkInterfaceType.Wireless80211, NetworkInterfaceType.Ethernet };

            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                for (int i = 0; i < interfaceTypes.Length; i++)
                {
                    if ((netInterface.NetworkInterfaceType == interfaceTypes[i] || netInterface.Name.ToLower().Contains("lan")) && netInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (UnicastIPAddressInformation ip in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }

            return LocalHost;
        }
    }
}