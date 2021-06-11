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
	public class CheckArea : MonoBehaviour
	{
		[SerializeField]
        protected Vector3 size = Vector3.one;
        public Vector3 Size { get { return size; } }

        [SerializeField]
        protected LayerMask mask = Physics.AllLayers;
        public LayerMask Mask { get { return mask; } }

        public virtual Collider[] Check()
        {
            return Physics.OverlapBox(transform.position, size / 2f, transform.rotation, mask, QueryTriggerInteraction.Ignore);
        }

        protected virtual void OnDrawGizmos()
        {
            var color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawCube(Vector3.zero, size);
        }
    }
}