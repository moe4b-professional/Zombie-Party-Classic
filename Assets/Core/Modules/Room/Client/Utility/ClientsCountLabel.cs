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

namespace Default
{
    [RequireComponent(typeof(Text))]
	public class ClientsCountLabel : MonoBehaviour
	{
        [SerializeField]
        protected string prefix;
        public string Prefix { get { return prefix; } }

        [SerializeField]
        protected string suffix;
        public string Suffix { get { return suffix; } }

        public Core Core { get { return Core.Asset; } }
        public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }
        public RoomCore Room { get { return Core.Room; } }

        Text label;

        void Start()
        {
            label = GetComponent<Text>();

            UpdateState();

            Room.JoinEvent += OnJoined;
            Room.DisconnectionEvent += OnDisconnection;
        }

        void UpdateState()
        {
            label.text = prefix + "(" + Room.Occupancy + "/" + WebSocketServer.Capacity + ")" + suffix;
        }

        void OnDisconnection(Client client)
        {
            UpdateState();
        }

        void OnJoined(Client client)
        {
            UpdateState();
        }

        void OnDestroy()
        {
            Room.JoinEvent -= OnJoined;
            Room.DisconnectionEvent -= OnDisconnection;
        }
    }
}