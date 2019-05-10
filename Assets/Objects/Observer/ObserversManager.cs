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
    public class ObserversManager : MonoBehaviour
    {
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        public List<Observer> List { get; protected set; }

        public virtual void Add(Observer observer)
        {
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

        public Level Level { get { return Level.Instance; } }
        public LevelMenu Menu { get { return Level.Menu; } }

        public Core Core { get { return Core.Asset; } }
        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }


        public virtual void Init()
        {
            List = new List<Observer>();

            Clients.DisconnectionEvent += OnDisconnnection;
        }

        void OnDisconnnection(Client client)
        {
            var observer = List.Find(x => x.Client == client);

            if (observer != null)
                Destroy(observer.gameObject);
        }

        public virtual void Spawn(Client client)
        {
            var instance = GameObject.Instantiate(prefab);

            var observer = instance.GetComponent<Observer>();
            observer.Init(client);

            instance.name = client.Name + " (" + observer.GetType().Name + ")";
        }

        void OnDestroy()
        {
            Clients.DisconnectionEvent -= OnDisconnnection;
        }
    }
}