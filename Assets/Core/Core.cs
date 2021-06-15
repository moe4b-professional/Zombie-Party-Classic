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

using MB;

namespace Default
{
    public abstract class CoreBase : ScriptableObject
    {
        public const string MenuPath = "Core" + "/";

        public static Core Asset { get; protected set; }

        #region Modules
        [SerializeField]
        UICore _UI = default;
        public UICore UI => _UI;

        [SerializeField]
        protected ScenesCore scenes;
        public ScenesCore Scenes { get { return scenes; } }

        [SerializeField]
        protected ServersCore servers;
        public ServersCore Servers { get { return servers; } }

        [SerializeField]
        protected RoomCore room;
        public RoomCore Room => room;

        [SerializeField]
        ScoresCore scores = default;
        public ScoresCore Scores => scores;

        [SerializeField]
        protected CheatsCore cheats;
        public CheatsCore Cheats { get { return cheats; } }

        public virtual void ForEachModule(Action<Core.Module> action)
        {
            action(UI);
            action(scenes);
            action(servers);
            action(room);
            action(scores);
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

        public SceneAccessor SceneAccessor { get; protected set; }

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

        protected virtual void Configure()
        {
            SceneAccessor = SceneAccessor.Create();

            OptionsOverride.Configure();

            ForEachModule(Process);
            void Process(Core.Module module) => module.Configure();

            SceneManager.sceneLoaded += OnSceneLoaded;
            Application.quitting += OnQuit;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Init();
        }

        public event Action OnInit;
        protected virtual void Init()
        {
            ForEachModule(Process);
            void Process(Core.Module module) => module.Init();

            LoadQualitySettings();

            if (OnInit != null) OnInit();
        }

        void LoadQualitySettings()
        {
            var id = OptionsOverride.Get("Quality", "Ultra").ToLower();

            var index = Array.FindIndex(QualitySettings.names, x => x.ToLower().Contains(id));

            if (index < 0)
            {
                Debug.LogError($"Quality Level: {id} Not Found");
                return;
            }

            QualitySettings.SetQualityLevel(index);
        }

        protected virtual void OnQuit()
        {
            servers.Stop();

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