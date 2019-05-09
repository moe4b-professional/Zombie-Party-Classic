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
    public class ObserversManager : MonoBehaviour
    {
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        public List<Observer> List { get; protected set; }

        public Observer Local { get; protected set; }

        public virtual void Set(Observer observer)
        {
            if (observer.isLocalPlayer)
                Local = observer;

            if (List.Contains(observer))
                throw new NotImplementedException();

            List.Add(observer);
        }

        public virtual void Remove(Observer observer)
        {
            if (!List.Contains(observer))
                throw new NotImplementedException();

            List.Remove(observer);
        }

        public virtual Observer Find(int connectionID)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].NetworkIdentity.connectionToClient.connectionId == connectionID)
                    return List[i];

            return null;
        }

        public short PlayerControllerID { get { return 0; } }

        public Level Level { get { return Level.Instance; } }
        public LevelMenu Menu { get { return Level.Menu; } }

        public Core Core { get { return Core.Asset; } }
        public NetworkCore Network { get { return Core.Server; } }


        public virtual void Init()
        {
            List = new List<Observer>();

            if (Network.Server.Active)
                Network.Server.AddPlayerEvent.Event += OnAddPlayer;
        }


        public virtual void Spawn()
        {
            ClientScene.AddPlayer(PlayerControllerID);
        }


        protected virtual void OnAddPlayer(NetworkMessage msg)
        {
            msg.reader.SeekZero();

            var addPlayerMsg = msg.ReadMessage<AddPlayerMessage>();

            if (addPlayerMsg.playerControllerId == PlayerControllerID)
            {
                var instance = GameObject.Instantiate(prefab);

                var observer = instance.GetComponent<Observer>();

                instance.name = Network.Players.Find(msg.conn.connectionId).Name + " (" + observer.GetType().Name + ")";

                NetworkServer.AddPlayerForConnection(msg.conn, instance, PlayerControllerID);
            }
        }


        protected virtual void OnDestroy()
        {
            Network.Server.AddPlayerEvent.Event -= OnAddPlayer;
        }
    }
}