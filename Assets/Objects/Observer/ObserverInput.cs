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

using Newtonsoft.Json;

namespace Game
{
	public class ObserverInput : MonoBehaviour, Observer.IReference
	{
        public Vector2 Move { get; protected set; }

        public Vector2 Look { get; protected set; }

        public Core Core { get { return Core.Asset; } }
        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }

        public LevelMenu Menu { get { return Level.Instance.Menu; } }

        Observer observer;

        public virtual void Init(Observer observer)
        {
            this.observer = observer;

            Clients.ClientNetworkMessageEvent.Add<InputMessage>(observer.Client, OnInput);
        }

        void OnInput(NetworkMessage msg)
        {
            var input = msg.To<InputMessage>();

            Move = input.Right;
            Look = input.Left;
        }
    }

    [NetworkMessage(10)]
    public class InputMessage : NetworkMessage
    {
        [JsonProperty]
        Vector2 left;
        [JsonIgnore]
        public Vector2 Left
        {
            get
            {
                return left;
            }
        }

        [JsonProperty]
        Vector2 right;
        [JsonIgnore]
        public Vector2 Right
        {
            get
            {
                return right;
            }
        }

        public override string ToString()
        {
            return right.ToString() + " : " + left.ToString();
        }

        public InputMessage(Vector2 right, Vector2 left)
        {
            this.right = right;
            this.left = left;
        }
    }
}