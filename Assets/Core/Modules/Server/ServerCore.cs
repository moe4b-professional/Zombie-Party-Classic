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

using UnityEngine.Networking;

using System.Net;
using System.Net.Http.Headers;

using System.Text.RegularExpressions;

using System.Threading;

using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game
{
    [CreateAssetMenu(menuName = MenuPath + "Asset")]
    public abstract class ServerCore : Core.Module
	{
        new public const string MenuPath = Core.Module.MenuPath + "Server/";

        public bool Active
        {
            get
            {
                return Server.IsListening;
            }
        }

        [SerializeField]
        protected int port = 8080;
        public int Port { get { return port; } }

        public string Address { get; protected set; }

        public WebSocketServer Server { get; protected set; }

        public ServerBehaviour Behaviour { get; protected set; }
        void InitBehaviour(ServerBehaviour behaviour)
        {
            this.Behaviour = behaviour;
        }
        public class ServerBehaviour : WebSocketBehavior
        {
            public ServerCore Server { get { return Core.Asset.Server; } }

            public delegate void ContextOperationDelegate(WebSocketContext context);

            protected override void OnOpen()
            {
                base.OnOpen();

                UnityThreadDispatcher.Add(OnOpen_UNITYSAFE(Context));
            }

            public event ContextOperationDelegate ConnectionEvent;
            IEnumerator OnOpen_UNITYSAFE(WebSocketContext context)
            {
                if (ConnectionEvent != null)
                    ConnectionEvent(Context);

                yield break;
            }


            protected override void OnMessage(MessageEventArgs e)
            {
                base.OnMessage(e);

                UnityThreadDispatcher.Add(OnMessage_UNITY_SAFE(e));
            }

            public delegate void MessageDelegate(WebSocketContext context, MessageEventArgs args);
            public event MessageDelegate MessageEvent;
            IEnumerator OnMessage_UNITY_SAFE(MessageEventArgs e)
            {
                if (MessageEvent != null) MessageEvent(Context, e);

                yield break;
            }


            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);

                UnityThreadDispatcher.Add(OnClose_UNITY_SAFE(Context, e));
            }

            public delegate void DisconnectOperationDelegate(WebSocketContext context, CloseEventArgs args);
            public event DisconnectOperationDelegate DisconnectionEvent;
            IEnumerator OnClose_UNITY_SAFE(WebSocketContext context, CloseEventArgs e)
            {
                if (DisconnectionEvent != null) DisconnectionEvent(Context, e);

                yield break;
            }

            public ServerBehaviour()
            {
                Server.InitBehaviour(this);
            }
        }

        public override void Configure()
        {
            base.Configure();

            Application.runInBackground = true;

            Core.SceneAccessor.ApplicationQuitEvent += OnApplicationQuit;
        }

        public override void Init()
        {
            base.Init();
        }

        public virtual void Start()
        {
            Address = GetLANIP();

            try
            {
                Server = new WebSocketServer(IPAddress.Any, port);

                Server.AddWebSocketService<ServerBehaviour>("/");

                Server.Log.Level = LogLevel.Error;
                Server.Log.Output = (data, s) => { Debug.LogError(data.Message); };

                Server.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Error when starting server, message: " + e.Message);
            }
        }

        
        public virtual void Stop()
        {
            
        }
        void OnApplicationQuit()
        {
            Stop();
        }


        public const string LocalHost = "127.0.0.1";
        public static string GetLANIP()
        {
            var interfaceTypes = new NetworkInterfaceType[] { NetworkInterfaceType.Wireless80211 };

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


        public abstract partial class Module : Core.Module
        {
            new public const string MenuPath = ServerCore.MenuPath + "Modules/";

            public ServerCore Server { get { return Core.Server; } }
        }
    }
}