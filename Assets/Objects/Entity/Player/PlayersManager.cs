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
using UnityEngine.Networking.NetworkSystem;

namespace Game
{
	public class PlayersManager : MonoBehaviour
	{
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        [SerializeField]
        protected Transform[] spawnPoints;
        public Transform[] SpawnPoints { get { return spawnPoints; } }

        public ConstraintArea ConstraintArea { get; protected set; }


        public List<Player> List { get; protected set; }
        public Player Find(Client client)
        {
            return List.Find(x => x.Client == client);
        }

        public virtual void Add(Player player)
        {
            if (List.Contains(player))
                throw new NotImplementedException();

            List.Add(player);

            ConstraintArea.AddTarget(player.transform);
        }

        public event Action<Player> OnRemove;
        public virtual void Remove(Player player)
        {
            if (!List.Contains(player))
                throw new NotImplementedException();

            ConstraintArea.RemoveTarget(player.transform);

            List.Remove(player);

            if (OnRemove != null) OnRemove(player);
        }


        public Level Level { get { return Level.Instance; } }
        public LevelMenu Menu { get { return Level.Menu; } }

        public Core Core { get { return Core.Asset; } }
        public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }


        public virtual void Init()
        {
            ConstraintArea = Dependancy.Get<ConstraintArea>(gameObject);

            List = new List<Player>();
        }

        public virtual Player Spawn(Observer observer)
        {
            var instance = GameObject.Instantiate(prefab);

            var player = instance.GetComponent<Player>();

            instance.name = observer.Client.Name + " (" + player.GetType().Name + ")";

            player.Init(observer);

            instance.transform.position = spawnPoints[observer.ID].position;
            instance.transform.rotation = spawnPoints[observer.ID].rotation;

            return player;
        }
    }
}