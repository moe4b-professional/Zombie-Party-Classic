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

namespace Default
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

        public event Action OnChange;
        protected virtual void InvokeChange()
        {
            if (OnChange != null) OnChange();
        }

        protected virtual void Reset()
        {
            Value = MaxValue;
        }

        public virtual void Add(float value)
        {
            this.Value += value;
        }

        public virtual void Remove(float value)
        {
            this.Value -= value;
        }
    }
}