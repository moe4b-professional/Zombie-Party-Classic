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

namespace Game
{
	public abstract class ToggleSet<T> : MonoBehaviour
	{
		[SerializeField]
        protected T off;
        public T Off { get { return off; } }

        [SerializeField]
        protected T on;
        public T On { get { return on; } }

        Toggle toggle;

        protected virtual void Awake()
        {
            toggle = Dependancy.Get<Toggle>(gameObject, Dependancy.Scope.RecursiveToParents);

            toggle.onValueChanged.AddListener(OnChange);
        }

        protected virtual void Start()
        {
            UpdateState(toggle.isOn);
        }

        protected virtual void OnChange(bool newValue)
        {
            UpdateState(newValue);
        }

        protected virtual void UpdateState(bool isOn)
        {
            if (isOn)
                Set(on);
            else
                Set(off);
        }

        protected abstract void Set(T value);
    }
}