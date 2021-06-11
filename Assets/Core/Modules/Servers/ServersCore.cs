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

using System.Net;

namespace Default
{
    [CreateAssetMenu(menuName = MenuPath + "Asset")]
	public class ServersCore : Core.Module
	{
        new public const string MenuPath = Core.Module.MenuPath + "Servers/";

        public IPAddress Address { get; protected set; }
        protected virtual void GetAddresss()
        {
            Address = LocalAddress.Get();

            try
            {
                var ID = "IP Address";

                if(OptionsOverride.Contains(ID))
                {
                    var text = OptionsOverride.Get<string>(ID);

                    Address = IPAddress.Parse(text);
                }
            }
            catch (Exception)
            {
                Debug.LogError("Error when getting IP Address, Using Local Address instead");
            };
        }

        #region Modules
        [SerializeField]
        protected WebSocketServerCore webSocket;
        public WebSocketServerCore WebSocket { get { return webSocket; } }

        [SerializeField]
        protected WebServerCore webServer;
        public WebServerCore WebServer { get { return webServer; } }

        [SerializeField]
        protected DNSCore _DNS;
        public DNSCore DNS { get { return _DNS; } }

        public virtual void ForEachModule(Action<Module> action)
        {
            action(webSocket);
            action(webServer);
            action(DNS);
        }
        #endregion

        public override void Configure()
        {
            base.Configure();

            ForEachModule(ConfigureModule);
        }
        protected virtual void ConfigureModule(Module module)
        {
            module.Configure();
        }

        public override void Init()
        {
            base.Init();

            ForEachModule(InitModule);
        }
        protected virtual void InitModule(Module module)
        {
            module.Init();
        }

        public virtual void Start()
        {
            GetAddresss();

            ForEachModule(StartModule);
        }
        protected virtual void StartModule(Module module)
        {
            module.Start();
        }

        public virtual void Stop()
        {
            ForEachModule(StopModule);
        }
        protected virtual void StopModule(Module module)
        {
            module.Stop();
        }

        public abstract class Module : Core.Module
        {
            new public const string MenuPath = ServersCore.MenuPath + "Modules/";

            public ServersCore Servers { get { return Core.Servers; } }

            public IPAddress Address { get { return Servers.Address; } }

            public abstract bool Active { get; }

            public virtual void Start()
            {

            }

            public virtual void Stop()
            {

            }
        }
	}
}