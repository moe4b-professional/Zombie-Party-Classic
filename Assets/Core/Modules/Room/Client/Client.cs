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

using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;

namespace Default
{
    [Serializable]
    public class Client
    {
        public string Name { get; protected set; }

        public int ID { get; protected set; }

        public bool IsReady { get; internal set; }

        public WSSBehaviour Behaviour { get; protected set; }
        public WebSocket Socket => Behaviour.Context.WebSocket;
        public WebSocketState State { get { return Socket.ReadyState; } }

        public virtual bool Send<T>(T message)
            where T : NetworkMessage
        {
            var json = NetworkMessage.Serialize(message);

            if (State != WebSocketState.Open)
            {
                Debug.LogError($"Cannot Send Message of {typeof(T).Name} When Client State is {State}");
                return false;
            }

            Socket.Send(json);
            return true;
        }

        public override string ToString() => Name;

        public Client(string name, int ID, WSSBehaviour behaviour)
        {
            this.Name = name;
            this.ID = ID;
            this.Behaviour = behaviour;
        }
    }
}