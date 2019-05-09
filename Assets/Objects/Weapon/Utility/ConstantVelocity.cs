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
    [RequireComponent(typeof(Rigidbody))]
	public class ConstantVelocity : MonoBehaviour
	{
        [SerializeField]
        protected Vector3 value;

        [SerializeField]
        protected bool relative = false;

        new Rigidbody rigidbody;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (relative)
                rigidbody.velocity = transform.TransformVector(value);
            else
                rigidbody.velocity = value;
        }
	}
}