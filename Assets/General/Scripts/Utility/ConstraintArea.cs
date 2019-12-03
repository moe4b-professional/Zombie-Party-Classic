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
	public class ConstraintArea : MonoBehaviour
	{
		[SerializeField]
        protected float length;
        public float Length { get { return length; } }

        [SerializeField]
        protected float width;
        public float Width { get { return width; } }

        [SerializeField]
        protected float height;
        public float Height { get { return height; } }

        [SerializeField]
        protected List<Transform> targets;
        public List<Transform> Target { get { return targets; } }

        public void AddTarget(Transform target)
        {
            targets.Add(target);
        }

        public virtual void RemoveTarget(Transform transform)
        {
            targets.Remove(transform);
        }

        void LateUpdate()
        {
            for (int i = 0; i < targets.Count; i++)
                Clamp(targets[i]);
        }

        void Clamp(Transform transform)
        {
            transform.position = Clamp(transform.position);
        }

        Vector3 Clamp(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);

            position.x = Mathf.Clamp(position.x, -width / 2f, width / 2f);
            position.y = Mathf.Clamp(position.y, -height / 2f, height / 2f);
            position.z = Mathf.Clamp(position.z, -length / 2f, length / 2f);

            position = transform.TransformPoint(position);

            return position;
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR

            Handles.matrix = transform.localToWorldMatrix;

            Handles.DrawWireCube(Vector3.zero, new Vector3(width, height, length));
#endif
        }
    }
}