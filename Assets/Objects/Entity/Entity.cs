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
    public class Entity : MonoBehaviour
    {
        public EntityHealth Health { get; protected set; }

        public virtual bool IsAlive { get { return Health.Value > 0f; } }
        public virtual bool IsDead { get { return Health.Value == 0f; } }

        protected virtual void Awake()
        {
            Health = GetComponent<EntityHealth>();
        }

        protected virtual void Start()
        {

        }

        public delegate void DamageDelegate(Entity damager, float value);
        public event DamageDelegate OnTookDamage;
        public virtual void TakeDamage(Entity damager, float value)
        {
            if (Health.Value <= 0f) return;

            Health.Value -= value;

            if (OnTookDamage != null) OnTookDamage(damager, value);

            if (Health.Value == 0f)
                Death(damager);
        }

        public delegate void DeathDelegate(Entity damager);
        public event DeathDelegate OnDeath;
        protected virtual void Death(Entity Damager)
        {
            if (OnDeath != null)
                OnDeath(Damager);
        }

        public virtual void Suicide()
        {
            this.TakeDamage(this, Health.Value);
        }
    }
}