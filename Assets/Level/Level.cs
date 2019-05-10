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


        public LevelMenu Menu { get; protected set; }

        public Popup Popup { get { return Menu.Popup; } }


        public LevelPause Pause { get; protected set; }


        public ObserversManager Observers { get; protected set; }

        public PlayersManager Players { get; protected set; }


        public Spawner Spawner { get; protected set; }


        public Core Core { get { return Core.Asset; } }

        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }

        void Awake()
        {
            Instance = this;

            Menu = FindObjectOfType<LevelMenu>();

            Observers = FindObjectOfType<ObserversManager>();
            Observers.Init();

            Players = FindObjectOfType<PlayersManager>();
            Players.Init();

            Spawner = FindObjectOfType<Spawner>();

            Pause = FindObjectOfType<LevelPause>();
            Pause.Init();
        }

		void Start()
        {

            Clients.ReadyStateChangedEvent += OnClientReadyStateChanged;
            Clients.DisconnectionEvent += OnClientDisconnection;

            Menu.Players.Visible = true;
            Menu.HUD.Visible = false;
        }

        void OnClientReadyStateChanged(Client client)
        {
            CheckReadiness();
        }

        void OnClientDisconnection(Client client)
        {
            CheckReadiness();
        }

        void CheckReadiness()
        {
            if (Clients.Count > 0 && Clients.AllReady)
            {
                Clients.ReadyStateChangedEvent -= OnClientReadyStateChanged;
                Clients.DisconnectionEvent -= OnClientDisconnection;

                //TODO
                Menu.Players.Visible = false;
                Menu.HUD.Visible = true;
                SpawnAllClients();
            }
        }

        void SpawnAllClients()
        {
            for (int i = 0; i < Clients.List.Count; i++)
            {
                Observers.Spawn(Clients.List[i]);
            }
        }

        void OnDestroy()
        {
            Clients.ReadyStateChangedEvent -= OnClientReadyStateChanged;
            Clients.DisconnectionEvent -= OnClientDisconnection;
        }
    }
}