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
    [RequireComponent(typeof(Text))]
	public class ClientsCountLabel : MonoBehaviour
	{
        [SerializeField]
        protected string prefix;
        public string Prefix { get { return prefix; } }

        [SerializeField]
        protected string suffix;
        public string Suffix { get { return suffix; } }

        public ServerCore Server { get { return Core.Asset.Server; } }

        public ClientsManagerCore Clients { get { return Core.Asset.Server.Clients; } }

        Text label;

        void Start()
        {
            label = GetComponent<Text>();

            UpdateState();

            Clients.JoinEvent += OnJoined;
            Clients.DisconnectionEvent += OnDisconnection;
        }

        void UpdateState()
        {
            label.text = prefix + "(" + Clients.Count + "/" + Server.Size + ")" + suffix;
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
            Clients.JoinEvent -= OnJoined;
            Clients.DisconnectionEvent -= OnDisconnection;
        }
    }
}