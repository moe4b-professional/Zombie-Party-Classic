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
using System.Xml;

namespace Game
{
    public abstract class CoreBase : ScriptableObject
    {
        public const string MenuPath = "Core" + "/";

        public static Core Asset { get; protected set; }

        #region Modules
        [SerializeField]
        protected ScenesCore scenes;
        public ScenesCore Scenes { get { return scenes; } }

        [SerializeField]
        protected ServerCore server;
        public ServerCore Server { get { return server; } }

        [SerializeField]
        protected WebServerCore webServer;
        public WebServerCore WebServer { get { return webServer; } }

        [SerializeField]
        protected CheatsCore cheats;
        public CheatsCore Cheats { get { return cheats; } }

        public virtual void ForEachModule(Action<Core.Module> action)
        {
            action(scenes);
            action(server);
            action(webServer);
            action(cheats);
        }

        public abstract class ModuleBase : ScriptableObject
        {
            public const string MenuPath = Core.MenuPath + "Modules/";

            public Core Core { get { return Core.Asset; } }

            public SceneAccessor SceneAccessor { get { return Core.SceneAccessor; } }

            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }
        #endregion

        #region Tools
        public SceneAccessor SceneAccessor { get; protected set; }
        protected virtual void ConfigureSceneAccessor()
        {
            SceneAccessor = SceneAccessor.Create();

            SceneAccessor.ApplicationQuitEvent += OnApplicationQuit;
        }
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnLoad()
        {
            Asset = Find();

            if(Asset == null)
            {
                Debug.LogWarning("No " + nameof(Core) + " asset found withing a Resources folder, Core operations will not work");
            }
            else
            {
                Asset.Configure();
            }
        }

        public static Core Find()
        {
            var cores = Resources.LoadAll<Core>("");

            foreach (var core in cores)
            {
                if (core.name.ToLower().Contains("override"))
                    return core;
            }

            if (cores.Length > 0)
                return cores.First();
            else
                return null;
        }

        #region Configure
        protected virtual void Configure()
        {
            ConfigureSceneAccessor();

            Initializer.Configure();

            OptionsOverride.Configure();

            ForEachModule(ConfigureModule);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void ConfigureModule(Core.Module module)
        {
            module.Configure();
        }
        #endregion

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            Init();
        }

        #region Init
        public event Action OnInit;
        protected virtual void Init()
        {
            ForEachModule(InitModule);

            if (OnInit != null) OnInit();
        }

        protected virtual void InitModule(Core.Module module)
        {
            module.Init();
        }
        #endregion

        protected virtual void OnApplicationQuit()
        {
            if (server.Active) server.Stop();

            if (webServer.Active) webServer.Stop();

            GC.Collect();
        }
    }

    [CreateAssetMenu(menuName = MenuPath + "Asset")]
	public partial class Core : CoreBase
    {
        public partial class Module : ModuleBase
        {
            
        }
    }
}