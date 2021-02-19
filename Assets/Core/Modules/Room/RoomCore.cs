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
    [CreateAssetMenu(menuName = MenuPath + "Room")]
    public class RoomCore : Core.Module
    {
        public List<Client> Clients { get; protected set; }

        public int Occupancy { get { return Clients.Count; } }

        protected virtual bool Contains(int ID)
        {
            for (int i = 0; i < Clients.Count; i++)
                if (Clients[i].ID == ID) return true;

            return false;
        }

        public WebSocketServerCore WebSocketServer => Core.Servers.WebSocket;

        protected virtual int GetVacantID()
        {
            for (int i = 0; i < WebSocketServer.Capacity; i++)
            {
                if (Contains(i)) continue;
                else return i;
            }

            throw new NotImplementedException("No More Vacant Slots Left To Populate");
        }

        public delegate void ClientOperationDelegate(Client client);

        #region Readiness
        public bool Ready
        {
            get
            {
                for (int i = 0; i < Clients.Count; i++)
                    if (Clients[i].IsReady == false) return false;

                return true;
            }
        }

        public event ClientOperationDelegate ReadyStateChangedEvent;
        public virtual void SetReadiness(Client client, bool value)
        {
            client.IsReady = value;

            if (ReadyStateChangedEvent != null) ReadyStateChangedEvent(client);
        }

        public virtual void SetAllReadiness(bool value)
        {
            for (int i = 0; i < Clients.Count; i++)
                SetReadiness(Clients[i], value);
        }
        #endregion

        public override void Configure()
        {
            base.Configure();

            Clients = new List<Client>();

            MessageDispatcher = new MessageDispatcherProperty();

            WebSocketServer.MessageEvent += OnMessage;
            WebSocketServer.DisconnectionEvent += OnDisconnection;
        }

        public event ClientOperationDelegate JoinEvent;
        void Register(string name, WSSBehaviour behaviour)
        {
            var id = GetVacantID();

            var client = new Client(name, id, behaviour);

            Clients.Add(client);

            if (JoinEvent != null) JoinEvent(client);
        }

        void OnMessage(WSSBehaviour behaviour, MessageEventArgs args)
        {
            if (args.Data == "ACKNOWLEDGE BYTES") return;

            var client = Clients.Find(x => x.Socket == behaviour.Context.WebSocket);

            NetworkMessage message;
            try
            {
                message = NetworkMessage.Deserialize(args.Data);
            }
            catch (Exception)
            {
                throw;
            }

            if (client == null)
            {
                if (message is RegisterClientMessage payload)
                    Register(payload.Name, behaviour);
                else
                    Debug.LogWarning($"Non Registered Client {behaviour.ID} Send Non Register Client Message: {message.GetType().Name}");
            }
            else
            {
                if (MessageDispatcher.Invoke(client, message) == false)
                {
                    if (message is ReadyClientMessage ready)
                        SetReadiness(client, ready.Value);
                }
            }
        }

        public MessageDispatcherProperty MessageDispatcher { get; protected set; }
        public class MessageDispatcherProperty
        {
            public Dictionary<(Client client, int id), Handler> Dictionary { get; protected set; }

            public delegate void Handler(NetworkMessage message);
            public delegate void Callback<T>(T message) where T : NetworkMessage;

            #region Add
            public void Add(Client target, int id, Handler handler)
            {
                Dictionary.Add((target, id), handler);
            }

            public void Add<TMessage>(Client target, Callback<TMessage> callback)
                where TMessage : NetworkMessage
            {
                var id = NetworkMessage.GetID<TMessage>();

                Add(target, id, Handler);

                void Handler(NetworkMessage message)
                {
                    var payload = message.To<TMessage>();

                    callback.Invoke(payload);
                }
            }
            #endregion

            #region Remove
            public bool Remove(int id, Client target)
            {
                return Dictionary.Remove((target, id));
            }

            public bool Remove<TNetworkMessage>(Client target)
                where TNetworkMessage : NetworkMessage
            {
                var id = NetworkMessage.GetID<TNetworkMessage>();

                return Remove(id, target);
            }

            public int RemoveAll(Client client)
            {
                var selection = Dictionary.Keys.Where(x => x.client == client).ToArray();

                var count = 0;

                for (int i = 0; i < selection.Length; i++)
                    if (Remove(selection[i].id, selection[i].client))
                        count += 1;

                return count;
            }
            #endregion

            public bool Invoke(Client client, NetworkMessage message)
            {
                if (Dictionary.TryGetValue((client, message.ID), out var handler) == false)
                    return false;

                handler.Invoke(message);
                return true;
            }

            public virtual void Clear()
            {
                Dictionary.Clear();
            }

            public MessageDispatcherProperty()
            {
                Dictionary = new Dictionary<(Client client, int id), Handler>();
            }
        }

        public event ClientOperationDelegate DisconnectionEvent;
        void OnDisconnection(WSSBehaviour behaviour, CloseEventArgs args)
        {
            var client = Clients.Find(x => x.Socket == behaviour.Context.WebSocket);

            if (client == null)
            {
                Debug.LogWarning($"Client {behaviour.ID} Disconnected Without being Registered");
                return;
            }

            Clients.Remove(client);

            MessageDispatcher.RemoveAll(client);

            DisconnectionEvent?.Invoke(client);
        }

        #region Brodcast
        public virtual void Broadcast<TMessage>(TMessage message)
            where TMessage : NetworkMessage
        {
            for (int i = 0; i < Clients.Count; i++)
                Clients[i].Send(message);
        }
        #endregion
    }
}