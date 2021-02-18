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
	public class PlayersListUI : UIElement
	{
        [SerializeField]
        protected Transform parent;
        public Transform Parent { get { return parent; } }

        [SerializeField]
        protected GameObject template;
        public GameObject Template { get { return template; } }

        [SerializeField]
        protected ColorsData colors = new ColorsData(Color.red, Color.green);
        public ColorsData Colors { get { return colors; } }
        [Serializable]
        public class ColorsData
        {
            [SerializeField]
            protected Color unready;
            public Color Unready { get { return unready; } }

            [SerializeField]
            protected Color ready;
            public Color Ready { get { return ready; } }

            public Color Get(bool isReady)
            {
                return isReady ? ready : unready;
            }

            public ColorsData(Color unready, Color ready)
            {
                this.unready = unready;
                this.ready = ready;
            }
        }

        public List<PlayersListElement> Elements { get; protected set; }

        public Core Core { get { return Core.Asset; } }
        public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }
        public RoomCore Room { get { return Core.Room; } }

        void Start()
        {
            Elements = new List<PlayersListElement>();

            Room.JoinEvent += OnJoin;
            Room.DisconnectionEvent += OnDisconnection;

            Room.ReadyStateChangedEvent += OnReadyStateChanged;

            foreach (var player in Room.Clients)
                if(player != null)
                    Add(player);
        }


        void OnReadyStateChanged(Client client)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if(Elements[i].Client == client)
                {
                    Elements[i].Color = colors.Get(client.IsReady);
                    break;
                }
            }
        }


        void OnJoin(Client client)
        {
            Add(client);
        }
        PlayersListElement Add(Client client)
        {
            var instance = Create(client);

            Elements.Add(instance);

            return instance;
        }
        PlayersListElement Create(Client client)
        {
            var instance = Instantiate(template, parent);

            var element = instance.GetComponent<PlayersListElement>();

            element.Set(client);

            element.Color = colors.Get(client.IsReady);

            return element;
        }


        void OnDisconnection(Client client)
        {
            Remove(client);
        }
        void Remove(Client client)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Client == client)
                {
                    Destroy(Elements[i].gameObject);
                    Elements.RemoveAt(i);
                    break;
                }
            }
        }


        void OnDestroy()
        {
            Room.JoinEvent -= OnJoin;
            Room.DisconnectionEvent -= OnDisconnection;

            Room.ReadyStateChangedEvent -= OnReadyStateChanged;
        }
    }
}