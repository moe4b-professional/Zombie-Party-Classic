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

namespace Game
{
    [DefaultExecutionOrder(ExecutionOrder)]
	public class Level : MonoBehaviour
	{
        public const int ExecutionOrder = -200;

        public static Level Instance { get; protected set; }

#pragma warning disable CS0109
        new public Camera camera { get; protected set; }
#pragma warning restore CS0109

        public LevelMenu Menu { get; protected set; }
        protected virtual void InitMenu()
        {
            Menu = FindObjectOfType<LevelMenu>();
            Menu.Init();
        }
        public Popup Popup { get { return Menu.Popup; } }


        public LevelPause Pause { get; protected set; }
        protected virtual void InitPause()
        {
            Pause = FindObjectOfType<LevelPause>();
            Pause.Init();
        }

        public LevelStartStage StartStage { get; protected set; }
        public LevelPlayStage PlayStage { get; protected set; }
        public LevelEndStage EndStage { get; protected set; }
        protected virtual void InitStages()
        {
            StartStage = FindObjectOfType<LevelStartStage>();
            StartStage.Init();

            PlayStage = FindObjectOfType<LevelPlayStage>();
            PlayStage.Init();

            EndStage = FindObjectOfType<LevelEndStage>();
            EndStage.Init();
        }

        public ObserversManager Observers { get; protected set; }
        protected virtual void InitObservers()
        {
            Observers = FindObjectOfType<ObserversManager>();
            Observers.Init();
        }

        public PlayersManager Players { get; protected set; }
        protected virtual void InitPlayers()
        {
            Players = FindObjectOfType<PlayersManager>();
            Players.Init();
        }

        public Spawner Spawner { get; protected set; }
        protected virtual void InitSpawner()
        {
            Spawner = FindObjectOfType<Spawner>();
        }

        public Core Core { get { return Core.Asset; } }
        public ScenesCore Scenes { get { return Core.Scenes; } }
        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }

        void Awake()
        {
            if (!Core.Server.Active)
            {
                Scenes.Load(Scenes.MainMenu.Name);
                enabled = false;
                return;
            }

            Instance = this;

            camera = Camera.main;

            InitMenu();

            InitObservers();

            InitPlayers();

            InitSpawner();

            InitStages();

            InitPause();
        }

		void Start()
        {
            EndStage.OnEnd += OnEnd;
            StartStage.Begin();
        }

        protected virtual void OnEnd()
        {
            
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}