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
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class NetworkMessage
    {
        [JsonProperty]
        public int ID { get; private set; }

        public TNetworkMessage To<TNetworkMessage>()
            where TNetworkMessage : NetworkMessage
        {
            return this as TNetworkMessage;
        }

        public NetworkMessage()
        {
            var type = GetType();

            ID = GetID(type);
        }

        //Static Utility

        public static Glossary<int, Type> Glossary { get; protected set; }

        public static Type GetType(int id)
        {
            if (Glossary.TryGetValue(id, out var type) == false)
                throw new Exception($"ID {id} Not Registered for Any Network Messages");

            return type;
        }

        public static int GetID<T>()
            where T : NetworkMessage
        {
            var type = typeof(T);

            return GetID(type);
        }
        public static int GetID(Type type)
        {
            if (Glossary.TryGetKey(type, out var id) == false)
                throw new Exception($"Type '{type}' Not Registered as Network Message");

            return id;
        }

        static void Register<T>(int id)
        {
            var type = typeof(T);

            Register(type, id);
        }
        static void Register(Type type, int id)
        {
            Glossary.Add(id, type);
        }

        public static NetworkMessage Deserialize(string json)
        {
            var jObject = JObject.Parse(json);

            return Deserialize(jObject);
        }
        public static NetworkMessage Deserialize(JObject json)
        {
            var ID = json["ID"].ToObject<int>();

            var type = GetType(ID);

            var message = (NetworkMessage)json.ToObject(type);

            return message;
        }

        public static string Serialize<T>(T message)
            where T : NetworkMessage
        {
            return JsonConvert.SerializeObject(message, JsonPersonal.Settings);
        }

        static NetworkMessage()
        {
            Glossary = new Glossary<int, Type>();

            var id = 0;

            void Add<T>()
                where T : NetworkMessage
            {
                Register<T>(id);

                id += 1;
            }

            Add<RegisterClientMessage>();
            Add<ReadyClientMessage>();

            Add<StartLevelMessage>();
            Add<RetryLevelMessage>();

            Add<PlayerInputMessage>();
            Add<PlayerHealthMessage>();

            Add<HitMarkerMessage>();

            Add<PingMessage>();
        }
    }

    #region Client
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RegisterClientMessage : NetworkMessage
    {
        [JsonProperty]
        string name = default;
        public string Name => name;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ReadyClientMessage : NetworkMessage
    {
        [JsonProperty]
        bool value = default;
        public bool Value => value;
    }
    #endregion

    #region Level
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class StartLevelMessage : NetworkMessage
    {

    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RetryLevelMessage : NetworkMessage
    {

    }
    #endregion

    #region Player
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerInputMessage : NetworkMessage
    {
        [JsonProperty]
        Vector2 left = default;
        public Vector2 Left => left;

        [JsonProperty]
        Vector2 right = default;
        public Vector2 Right => right;

        public override string ToString() => $"{right} : {left}";
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerHealthMessage : NetworkMessage
    {
        [JsonProperty]
        float value;
        public float Value { get { return value; } }

        [JsonProperty]
        float max;
        public float Max { get { return max; } }

        public override string ToString() => $"{value} / {max}";

        public PlayerHealthMessage(float value, float max)
        {
            this.value = value;
            this.max = max;
        }
    }
    #endregion

    public class HitMarkerMessage : NetworkMessage
    {
        [JsonProperty]
        bool hit = default;
        public bool Hit => hit;

        [JsonProperty]
        float[] pattern = default;
        public float[] Pattern => pattern;

        public HitMarkerMessage(bool hit, params float[] duration)
        {
            this.hit = hit;
            this.pattern = duration;
        }
    }

    public class PingMessage : NetworkMessage
    {
        [JsonProperty]
        long stamp = default;
        public long Stamp => stamp;
    }
}