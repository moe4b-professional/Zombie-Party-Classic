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
	public class UITransition : MonoBehaviour
	{
        public CanvasGroup Group { get; protected set; }

        [SerializeField]
        [Range(0f, 1f)]
        protected float _value;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                value = Mathf.Clamp01(value);

                this._value = value;

                if (OnValueChanged != null) OnValueChanged(Value);
            }
        }
        public event Action<float> OnValueChanged;

        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        public virtual void Init()
        {
            Group = GetComponent<CanvasGroup>();
        }

        public virtual void To(float target)
        {
            if (target > 0f && !gameObject.activeInHierarchy) gameObject.SetActive(true);

            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(Procedure(target));
        }

        Coroutine coroutine;
        IEnumerator Procedure(float target)
        {
            while (Value != target)
            {
                Value = Mathf.MoveTowards(Value, target, speed * Time.unscaledDeltaTime);
                yield return null;
            }

            coroutine = null;

            if (Value == 0f && gameObject.activeInHierarchy) gameObject.SetActive(false);
        }

        public abstract class Module : MonoBehaviour, Initializer.Interface
        {
            protected UITransition transition;

            public virtual void Init()
            {
                transition = Dependancy.Get<UITransition>(gameObject, Dependancy.Scope.RecursiveToParents);

                if (transition == null)
                    throw new NullReferenceException("Cannot find UI Transition for module: " + name + " to use");

                UpdateState(transition.Value);

                transition.OnValueChanged += OnValueChanged;
            }

            protected virtual void UpdateState(float value)
            {

            }

            protected virtual void OnValueChanged(float value)
            {
                UpdateState(value);
            }
        }
	}
}