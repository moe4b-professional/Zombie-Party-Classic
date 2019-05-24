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

#pragma warning disable CS0109
        new Rigidbody rigidbody;
#pragma warning restore CS0109

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