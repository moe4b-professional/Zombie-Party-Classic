using System;
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
    public class EntityHealth : MonoBehaviour
    {
        [SerializeField]
        protected float _value;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                value = Mathf.Clamp(value, 0f, MaxValue);

                this._value = value;

                if (OnValueChanged != null) OnValueChanged(this._value);

                InvokeChange();
            }
        }

        public event Action<float> OnValueChanged;

        [SerializeField]
        protected float _maxValue = 100;
        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (value < 0f) value = 0f;

                _maxValue = value;

                if (OnMaxValueChanged != null) OnMaxValueChanged(this._value);

                InvokeChange();
            }
        }

        public event Action<float> OnMaxValueChanged;

        public event Action OnChanged;
        protected virtual void InvokeChange()
        {
            if (OnChanged != null) OnChanged();
        }

        protected virtual void Reset()
        {
            Value = MaxValue;
        }

        public delegate void DamageDelegate(Entity damager, float value);
        public event DamageDelegate OnDamage;
        public virtual void Damage(Entity damager, float value)
        {
            if (this.Value <= 0f) return;

            this.Value -= value;

            if (OnDamage != null) OnDamage(damager, value);

            if (this.Value == 0f)
                Death(damager);
        }

        public delegate void DeathDelegate(Entity damager);
        public event DeathDelegate OnDeath;
        protected virtual void Death(Entity Damager)
        {
            if (OnDeath != null)
                OnDeath(Damager);
        }

        public virtual void Add(float value)
        {
            this.Value += value;
        }
    }
}