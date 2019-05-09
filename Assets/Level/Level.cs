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

using UnityEngine.Networking;

namespace Game
{
    [DefaultExecutionOrder(ExecutionOrder)]
	public class Level : NetworkBehaviour
	{
        public const int ExecutionOrder = -200;

        public static Level Instance { get; protected set; }


        public LevelMenu Menu { get { return LevelMenu.Instance; } }

        public Popup Popup { get { return Menu.Popup; } }


        public LevelPause Pause { get; protected set; }


        public ObserversManager Observers { get; protected set; }

        public PlayersManager Players { get; protected set; }


        public Spawner Spawner { get; protected set; }


        public Core Core { get { return Core.Asset; } }

        public NetworkCore Network { get { return Core.Server; } }


        void Init()
        {
            Instance = this;

            Observers = FindObjectOfType<ObserversManager>();
            Observers.Init();

            if (Network.Server.Active)
            {
                Players = FindObjectOfType<PlayersManager>();
                Players.Init();
            }

            if(Network.Server.Active)
            {
                Spawner = FindObjectOfType<Spawner>();
                Spawner.Begin();
            }

            Pause = FindObjectOfType<LevelPause>();
            Pause.Init();
        }

		void Start()
        {
            Init();

            if (isServer)
            {
                Menu.Server.Players.Visible = false;
                Menu.Server.HUD.Visible = true;
            }

            if(isClient)
            {
                Popup.Visible = false;

                Menu.Client.Ready.Visible = false;
                Menu.Client.HUD.Visible = true;

                Observers.Spawn();
            }
        }
    }
}