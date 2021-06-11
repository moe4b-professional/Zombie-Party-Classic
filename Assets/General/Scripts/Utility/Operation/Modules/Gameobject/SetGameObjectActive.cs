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
	public class SetGameObjectActive : Operation.Behaviour
	{
		[SerializeField]
        protected GameObject target;
        public GameObject Target { get { return target; } }

        [SerializeField]
        protected bool value = false;
        public bool Value { get { return value; } }

        protected virtual void Reset()
        {
            target = gameObject;
        }

        public override void Execute()
        {
            target.SetActive(value);
        }
    }
}