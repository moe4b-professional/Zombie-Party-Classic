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

using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;

namespace Game
{
    [CreateAssetMenu(menuName = MenuPath + "Clients")]
	public class ClientsManagerCore : ServerCore.Module
	{
        public List<Client> List { get; protected set; }

        protected virtual int GetVacantID()
        {
            for (int i = 0; i < Server.Size; i++)
            {
                if (Contains(i)) continue;
                else return i;
            }

            throw new NotImplementedException("No More Vacant Slots Left To Populate");
        }
        protected virtual bool Contains(int ID)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].ID == ID) return true;

            return false;
        }

        public int Count { get { return List.Count; } }

        public bool AllReady
        {
            get
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i].isReady == false) return false;

                return true;
            }
        }

        public delegate void ClientOperationDelegate(Client client);

        public override void Configure()
        {
            base.Configure();

            List = new List<Client>();

            ClientNetworkMessageEvent = new NetworkMessageDispatcher();

            Server.MessageEvent += OnInternalMessage;
            Server.DisconnectionEvent += OnDisconnection;
        }

        void OnInternalMessage(WebSocketContext context, MessageEventArgs args)
        {
            var client = List.Find(x => x.Socket == context.WebSocket);

            if (client == null)
            {
                if (args.IsText)
                {
                    if (Header.IsValid(args.Data))
                    {
                        var header = Header.Parse(args.Data);

                        if (header.Key == "player name")
                            OnJoined(context, header.Value);
                    }
                }
            }
            else
                OnMessage(client, args);
        }

        public event ClientOperationDelegate JoinEvent;
        void OnJoined(WebSocketContext context, string name)
        {
            var client = new Client(name, GetVacantID(), context);

            List.Add(client);

            if (JoinEvent != null) JoinEvent(client);
        }

        public NetworkMessageDispatcher ClientNetworkMessageEvent { get; protected set; }
        public class NetworkMessageDispatcher
        {
            public delegate void Delegate(NetworkMessage msg);

            public List<Listener> Listeners { get; protected set; }
            public class Listener
            {
                public int ID { get; private set; }

                public Client Client { get; private set; }

                public Delegate Callback { get; private set; }

                public void Invoke(NetworkMessage msg)
                {
                    Callback.Invoke(msg);
                }

                public virtual bool Compare(int ID, Client client, Delegate callback)
                {
                    return this.ID == ID && this.Client == client && this.Callback == callback;
                }

                public Listener(int ID, Client client, Delegate callback)
                {
                    this.ID = ID;
                    this.Client = client;
                    this.Callback = callback;
                }
            }

            public delegate void GlobalDelegate(int ID, Client client, NetworkMessage msg);
            public event GlobalDelegate GlobalEvent;

            public void Add(int ID, Client target, Delegate callback)
            {
                for (int i = 0; i < Listeners.Count; i++)
                    if (Listeners[i].Compare(ID, target, callback))
                        throw new ArgumentException("Listener with ID: " + ID + " And Callback: " + callback.Method.Name + " Already Registered");

                Listeners.Add(new Listener(ID, target, callback));
            }
            public void Add<TNetworkMessage>(Client target, Delegate callback)
                where TNetworkMessage : NetworkMessage
            {
                Add(NetworkMessage.GetID<TNetworkMessage>(), target, callback);
            }

            public void Remove(int ID, Client target, Delegate callback)
            {
                for (int i = 0; i < Listeners.Count; i++)
                {
                    if (Listeners[i].Compare(ID, target, callback))
                    {
                        Listeners.RemoveAt(i);
                        return;
                    }
                }

                throw new ArgumentException("No Listener with ID: " + ID + " And Callback: " + callback.Method.Name + " Was Found");
            }
            public void Remove<TNetworkMessage>(Client target, Delegate callback)
                where TNetworkMessage : NetworkMessage
            {
                Remove(NetworkMessage.GetID<TNetworkMessage>(), target, callback);
            }
            public void RemoveAll(Client client)
            {
                Listeners.RemoveAll(x => x.Client == client);
            }

            public void Invoke(Client client, NetworkMessage msg)
            {
                for (int i = 0; i < Listeners.Count; i++)
                    if (Listeners[i].ID == msg.ID && Listeners[i].Client == client)
                        Listeners[i].Invoke(msg);

                if (GlobalEvent != null) GlobalEvent(msg.ID, client, msg);
            }

            public virtual void Clear()
            {
                Listeners.Clear();
            }

            public NetworkMessageDispatcher()
            {
                Listeners = new List<Listener>();
            }
        }

        public event ClientOperationDelegate ReadyStateChangedEvent;

        void OnMessage(Client client, MessageEventArgs args)
        {
            if (args.IsText)
            {
                //Debug.Log(args.Data);

                if (Header.IsValid(args.Data)) //is a header
                {
                    var header = Header.Parse(args.Data);

                    if (header.Key == "ready")
                    {
                        client.isReady = bool.Parse(header.Value);
                        if (ReadyStateChangedEvent != null) ReadyStateChangedEvent(client);
                    }
                }
                else if(args.Data.Contains("\"ID\":")) //is a networkmessage .... hopefully
                {
                    try
                    {
                        var msg = NetworkMessage.Get(args.Data);

                        ClientNetworkMessageEvent.Invoke(client, msg);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public event ClientOperationDelegate DisconnectionEvent;
        void OnDisconnection(WebSocketContext context, CloseEventArgs args)
        {
            var client = List.Find(x => x.Socket == context.WebSocket);

            if (client == null)
                Debug.LogWarning("WebSocket: " + context.UserEndPoint + " disconnected without getting registered as a client, most likely the client didn't send it's name");
            else
            {
                List.Remove(client);

                ClientNetworkMessageEvent.RemoveAll(client);

                if (DisconnectionEvent != null) DisconnectionEvent(client);
            }
        }
    }
}