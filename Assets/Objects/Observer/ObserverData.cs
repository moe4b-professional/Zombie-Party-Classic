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

namespace Game
{
	public class ObserverData : NetworkBehaviour, Observer.IReference
    {
        public Level Level { get { return Level.Instance; } }

        public LevelMenu Menu { get { return Level.Menu; } }

        Observer observer;

        public virtual void Init(Observer observer)
        {
            this.observer = observer;
        }

        #region Health
        [SyncVar(hook = "OnHealthChanged")]
        protected float health;
        public float Health { get { return health; } }

        public const float MaxHealth = 100f;

        [Server]
        public virtual void SetHealth(float value)
        {
            health = value;
        }

        public event Action<float> HealthChangeEvent;
        protected virtual void OnHealthChanged(float value)
        {
            health = value;

            if(isLocalPlayer)
                Menu.Client.HUD.HealthBar.Value = value / MaxHealth;

            if (HealthChangeEvent != null) HealthChangeEvent(health);
        }

        [Server]
        public virtual void InvokeDamage(float value)
        {
            RpcDamage(value);
        }

        [ClientRpc]
        protected virtual void RpcDamage(float value)
        {
#if UNITY_ANDROID
            if(isLocalPlayer)
                Handheld.Vibrate();
#endif
        }
#endregion
    }
}