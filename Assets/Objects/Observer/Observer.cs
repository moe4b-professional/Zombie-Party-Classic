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
    [RequireComponent(typeof(ObserverInput))]
	public class Observer : NetworkBehaviour
    {
        public Level Level { get { return Level.Instance; } }
        public virtual ObserversManager Manager { get { return Level.Observers; } }

        public Core Core { get { return Core.Asset; } }
        public NetworkCore Network { get { return Core.Server; } }

        public NetworkIdentity NetworkIdentity { get; protected set; }

        public ObserverInput Input { get; protected set; }

        public ObserverData Data { get; protected set; }

        public interface IReference : References.Interface<Observer>
        {

        }

        public int ID { get; set; }
        protected virtual void SetID()
        {
            if (isLocalPlayer)
            {
                ID = Network.Client.ID;
            }

            if (isServer)
            {
                ID = Network.Players.GetIndexOf(connectionToClient.connectionId);
            }
        }

        protected virtual void Awake()
        {
            NetworkIdentity = GetComponent<NetworkIdentity>();
        }

        protected virtual void Start()
        {
            SetID();

            Input = GetComponent<ObserverInput>();

            Data = GetComponent<ObserverData>();

            Manager.Set(this);

            References.Init(this);

            if (isLocalPlayer)
            {
                ClientScene.AddPlayer(Player.ControllerID);
            }
        }

        public event Action DestroyEvent;
        void OnDestroy()
        {
            Manager.Remove(this);

            if (DestroyEvent != null) DestroyEvent();
        }
    }
}