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
    [DefaultExecutionOrder(-200)]
	public class SandboxBehaviour : MonoBehaviour
	{
        public float angle;

        public Transform a;
        public Transform b;

        private void Start()
        {
            
        }

        private void Update()
        {
            angle = Vector3.Angle(a.forward, (b.position - a.position).normalized);
        }
    }
}