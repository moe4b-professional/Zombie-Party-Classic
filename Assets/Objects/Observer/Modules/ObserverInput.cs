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
        public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }
        public RoomCore Room { get { return Core.Room; } }

        public LevelMenu Menu { get { return Level.Instance.Menu; } }

        Observer observer;

        public virtual void Init(Observer observer)
        {
            this.observer = observer;
        }

        void Start()
        {
            Room.MessageDispatcher.Add<PlayerInputMessage>(observer.Client, OnInput);
        }

        void OnInput(PlayerInputMessage input)
        {
            Move = input.Left;
            Look = input.Right;
        }

        void OnDestroy()
        {
            Room.MessageDispatcher.Remove<PlayerInputMessage>(observer.Client);
        }
    }
}