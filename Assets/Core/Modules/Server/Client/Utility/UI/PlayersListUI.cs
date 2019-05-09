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

            public ColorsData(Color unready, Color ready)
            {
                this.unready = unready;
                this.ready = ready;
            }
        }

        public List<PlayersListElement> Elements { get; protected set; }

        public NetworkCore Network { get { return Core.Asset.Server; } }
        public ClientsNetworkCore Players { get { return Core.Asset.Server.Players; } }

        void Start()
        {
            Elements = new List<PlayersListElement>();

            Players.OnAdd += OnAdd;
            Players.OnRemove += OnRemove;

            Network.Server.ClientReadyEvent.Event += OnClientReady;

            foreach (var player in Players.List)
                if(player != null)
                    Add(player);
        }


        void OnClientReady(UnityEngine.Networking.NetworkMessage msg)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if(Elements[i].Data.ID == msg.conn.connectionId)
                {
                    Elements[i].Color = colors.Ready;
                    break;
                }
            }
        }

        void OnAdd(ClientsNetworkCore.Data target)
        {
            Add(target);
        }
        PlayersListElement Add(ClientsNetworkCore.Data data)
        {
            var instance = Create(data);

            Elements.Add(instance);

            return instance;
        }


        void OnRemove(ClientsNetworkCore.Data target)
        {
            Remove(target);
        }
        void Remove(ClientsNetworkCore.Data data)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Data == data)
                {
                    Destroy(Elements[i].gameObject);
                    Elements.RemoveAt(i);
                    break;
                }
            }
        }


        void OnDestroy()
        {
            Players.OnAdd -= OnAdd;
            Players.OnRemove -= OnRemove;

            Network.Server.ClientReadyEvent.Event -= OnClientReady;
        }

        PlayersListElement Create(ClientsNetworkCore.Data data)
        {
            var instance = Instantiate(template, parent);

            var element = instance.GetComponent<PlayersListElement>();

            element.Set(data);

            element.Color = colors.Unready;

            return element;
        }
    }
}