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
	public class ObserverData : MonoBehaviour, Observer.IReference
    {
        public Level Level { get { return Level.Instance; } }

        public LevelMenu Menu { get { return Level.Menu; } }

        Observer observer;
        public Client Client { get { return observer.Client; } }

        public virtual void Init(Observer observer)
        {
            this.observer = observer;
        }

        public virtual void UpdateHealth(EntityHealth health)
        {
            if (Client.State != WebSocketSharp.WebSocketState.Open) return;

            Client.Send(new HealthMessage(health));
        }
    }

    [NetworkMessage(11)]
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
        public HealthMessage(EntityHealth health) : 
            this(health.Value, health.MaxValue)
        {

        }
    }
}