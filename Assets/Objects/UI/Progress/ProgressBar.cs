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

namespace Default
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        protected float _value = 0.5f;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                value = Mathf.Clamp01(value);

                _value = value;

                UpdateModules(_value);

                if (OnChange != null) OnChange(_value);
            }
        }

        public delegate void ChangeDelegate(float value);
        public event ChangeDelegate OnChange;

        protected virtual void UpdateModules(float value)
        {
            var dependancies = Dependancy.GetAll<Module>(gameObject);

            foreach (var dependancy in dependancies)
                dependancy.OnValueChanged(value);
        }

        public class Module : MonoBehaviour, Initializer.Interface
        {
            [SerializeField]
            [HideInInspector]
            public ProgressBar Bar { get; protected set; }


            protected virtual void Reset()
            {
                GetDependancies();

                UpdateValue();
            }

            public virtual void Init()
            {
                GetDependancies();

                UpdateValue();
            }


            protected virtual void GetDependancies()
            {
                Bar = Dependancy.Get<ProgressBar>(gameObject, Dependancy.Scope.RecursiveToParents);
            }


            public virtual void OnValueChanged(float value)
            {
                SetValue(value);
            }

            protected virtual void UpdateValue()
            {
                SetValue(Bar.Value);
            }

            protected virtual void SetValue(float value)
            {

            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ProgressBar))]
        public class Inspector : Editor
        {
            new ProgressBar target;

            void OnEnable()
            {
                target = base.target as ProgressBar;
            }

            public override void OnInspectorGUI()
            {
                var value = target.Value;

                value = EditorGUILayout.Slider("Value", value, 0f, 1f);

                if (target.Value != value)
                    target.Value = value;
            }
        }
#endif
    }
}