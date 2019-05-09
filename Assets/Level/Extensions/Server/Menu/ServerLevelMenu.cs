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
	public class ServerLevelMenu : Menu
	{
        [SerializeField]
        protected Menu players;
        public Menu Players { get { return players; } }

        [SerializeField]
        protected Menu _HUD;
        public Menu HUD { get { return _HUD; } }

        public LevelMenu GameMenu { get { return LevelMenu.Instance; } }
        public Popup Popup { get { return GameMenu.Popup; } }

        public Core Core { get { return Core.Asset; } }
        public NetworkCore Network { get { return Core.Server; } }


        void OnEnable()
        {
            players.Visible = true;
        }

        void Start()
        {
            Network.Server.ClientReadyEvent.Event += OnClientReady;
            Network.Server.ClientDisconnectedEvent.Event += OnClientDisconnected;
        }


        void OnClientReady(UnityEngine.Networking.NetworkMessage msg)
        {
            CheckAllConnectionsReady();
        }

        void OnClientDisconnected(UnityEngine.Networking.NetworkMessage msg)
        {
            CheckAllConnectionsReady();
        }

        bool CheckAllConnectionsReady()
        {
            if(Network.Server.ConnectionsCount > 0 && Network.Server.AllConnectionsReady)
            {
                Network.Server.ClientReadyEvent.Event -= OnClientReady;
                Network.Server.ClientDisconnectedEvent.Event -= OnClientDisconnected;

                Network.Server.SpawnObjects();

                return true;
            }
            else
            {
                return false;
            }
        }

        void OnDestroy()
        {
            Network.Server.ClientReadyEvent.Event -= OnClientReady;
            Network.Server.ClientDisconnectedEvent.Event -= OnClientDisconnected;
        }
    }
}