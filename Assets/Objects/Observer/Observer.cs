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
    [RequireComponent(typeof(ObserverInput))]
	public class Observer : MonoBehaviour
    {
        public Level Level { get { return Level.Instance; } }
        public virtual ObserversManager Manager { get { return Level.Observers; } }

        public Core Core { get { return Core.Asset; } }

        public Client Client { get; protected set; }
        public int ID { get { return Client.ID; } }

        public ObserverInput Input { get; protected set; }
        public ObserverData Data { get; protected set; }

        public interface IReference : References.Interface<Observer>
        {

        }

        public virtual void Init(Client client)
        {
            this.Client = client;

            Input = GetComponent<ObserverInput>();

            Data = GetComponent<ObserverData>();

            Manager.Add(this);

            References.Init(this);

            Level.Players.Spawn(this);
        }
    }
}