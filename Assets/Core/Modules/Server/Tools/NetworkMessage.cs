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

using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Game
{
    public abstract class NetworkMessage
    {
        public int ID { get; private set; }

        public TNetworkMessage To<TNetworkMessage>()
            where TNetworkMessage : NetworkMessage
        {
            return this as TNetworkMessage;
        }

        public NetworkMessage()
        {
            ID = GetID(GetType());
        }

        //------------------------------------------------------------------------

        public static void Configure()
        {
            Types.Configure();
        }

        public static class Types
        {
            public static List<Data> List { get; private set; }
            [Serializable]
            public class Data
            {
                public int ID { get; private set; }

                public string Name { get; private set; }

                [JsonIgnore]
                public Type Type { get; private set; }

                public override string ToString()
                {
                    return Name + " / " + ID;
                }

                public Data(int ID, Type type)
                {
                    this.ID = ID;

                    this.Type = type;

                    this.Name = type.Name;
                }
            }

            public static Data Find(int ID)
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i].ID == ID)
                        return List[i];

                return null;
            }
            public static Data Find(Type type)
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i].Type == type)
                        return List[i];

                return null;
            }
            public static Data Find<T>()
                where T : NetworkMessage
            {
                return Find(typeof(T));
            }

            public static void Configure()
            {
                List = GetAll();
            }

            public static List<Data> GetAll()
            {
                var list = new List<Data>();

                var IDs = new Dictionary<int, Data>();

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        var attributes = type.GetCustomAttributes(typeof(NetworkMessageAttribute), true);

                        if (attributes.Length == 0) continue;

                        var attribute = (NetworkMessageAttribute)attributes.First();

                        if (IDs.ContainsKey(attribute.ID))
                            throw new InvalidDataException("NetworkMessage ID: " + attribute.ID + " is assigned to both: " + type.Name + " & " + IDs[attribute.ID].Name);

                        var entry = new Data(attribute.ID, type);

                        list.Add(entry);
                        IDs.Add(entry.ID, entry);
                    }
                }

                return list;
            }
        }

        public static int GetID(Type type)
        {
            var data = Types.Find(type);

            if(data == null)
                throw new InvalidDataException("Type: " + type.Name + " Not Registerd As A Network Message");

            return data.ID;
        }
        public static int GetID<TNetworkMessage>()
            where TNetworkMessage : NetworkMessage
        {
            var data = Types.Find<TNetworkMessage>();

            if (data == null)
                throw new InvalidDataException("Type: " + typeof(TNetworkMessage).Name + " Not Registerd As A Network Message");

            return data.ID;
        }

        public static TNetworkMessage Get<TNetworkMessage>(string json)
            where TNetworkMessage : NetworkMessage
        {
            return Get<TNetworkMessage>(JObject.Parse(json));
        }
        public static TNetworkMessage Get<TNetworkMessage>(JObject json)
            where TNetworkMessage : NetworkMessage
        {
            var ID = json["ID"].ToObject<int>();

            var data = Types.Find(ID);

            if (data == null)
                throw new InvalidDataException("No Network Message registered with ID: " + ID);

            var message = (TNetworkMessage)json.ToObject(data.Type);

            message.ID = ID;

            return message;
        }
        public static NetworkMessage Get(string json)
        {
            return Get(JObject.Parse(json));
        }
        public static NetworkMessage Get(JObject json)
        {
            return Get<NetworkMessage>(json);
        }

        public static string Serialize(NetworkMessage message)
        {
            return JsonConvert.SerializeObject(message, JsonPersonal.Settings);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    sealed class NetworkMessageAttribute : Attribute
    {
        public readonly int _ID;
        public int ID { get { return _ID; } }

        public NetworkMessageAttribute(int ID)
        {
            this._ID = ID;
        }
    }

    [NetworkMessage(1)]
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

    [NetworkMessage(2)]
    public class HealthMessage : NetworkMessage
    {
        [JsonProperty]
        float value;
        [JsonIgnore]
        public float Value { get { return value; } }

        [JsonProperty]
        float max;
        [JsonIgnore]
        public float Max { get { return max; } }

        public override string ToString()
        {
            return value + "/" + max;
        }

        public HealthMessage(float value, float max)
        {
            this.value = value;
            this.max = max;
        }
    }
}