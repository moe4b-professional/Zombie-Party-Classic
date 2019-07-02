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

namespace Game
{
    [Serializable]
    public class Client
    {
        public string Name { get; protected set; }

        public bool isReady;

        public int ID { get; protected set; }

        public WebSocket Socket { get; protected set; }
        public WebSocketState State { get { return Socket.ReadyState; } }
        public virtual void Send(string data)
        {
            if(State != WebSocketState.Open)
            {
                Debug.LogError("Trying to send message to unopen client socket, message: " + data);
                return;
            }

            Socket.Send(data);
        }
        public virtual void Send(NetworkMessage message)
        {
            Send(NetworkMessage.Serialize(message));
        }

        public override string ToString()
        {
            return Name;
        }

        public Client(string name, int ID, WebSocketContext context)
        {
            this.Name = name;
            this.ID = ID;
            this.Socket = context.WebSocket;
        }
    }
}