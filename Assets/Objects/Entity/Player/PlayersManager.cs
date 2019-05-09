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

        public virtual void Set(Player player)
        {
            if (List.Contains(player))
                throw new NotImplementedException();

            List.Add(player);

            ConstraintArea.AddTarget(player.transform);
        }

        public virtual void Remove(Player player)
        {
            if (!List.Contains(player))
                throw new NotImplementedException();

            ConstraintArea.RemoveTarget(player.transform);

            List.Remove(player);
        }

        public static short PlayerControllerID { get { return Player.ControllerID; } }

        public Level Level { get { return Level.Instance; } }
        public LevelMenu Menu { get { return Level.Menu; } }

        public Core Core { get { return Core.Asset; } }
        public NetworkCore Network { get { return Core.Server; } }


        public virtual void Init()
        {
            ConstraintArea = Dependancy.Get<ConstraintArea>(gameObject);

            List = new List<Player>();

            if (Network.Server.Active)
                Network.Server.AddPlayerEvent.Event += OnAddPlayer;
        }


        protected virtual void OnAddPlayer(NetworkMessage msg)
        {
            msg.reader.SeekZero();

            var addPlayerMsg = msg.ReadMessage<AddPlayerMessage>();

            if (addPlayerMsg.playerControllerId == PlayerControllerID)
            {
                var instance = GameObject.Instantiate(prefab);

                var player = instance.GetComponent<Player>();

                var observer = Level.Observers.Find(msg.conn.connectionId);

                instance.name = Network.Players.Find(msg.conn.connectionId).Name + " (" + player.GetType().Name + ")";

                player.Init(observer);

                instance.transform.position = spawnPoints[observer.ID].position;
                instance.transform.rotation = spawnPoints[observer.ID].rotation;
            }
        }


        protected virtual void OnDestroy()
        {
            Network.Server.AddPlayerEvent.Event -= OnAddPlayer;
        }
    }
}