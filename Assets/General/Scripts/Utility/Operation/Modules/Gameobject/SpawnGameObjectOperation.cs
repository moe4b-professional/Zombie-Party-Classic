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
	public class SpawnGameObjectOperation : Operation.Behaviour
	{
		[SerializeField]
        protected GameObject source;
        public GameObject Source { get { return source; } }

        [SerializeField]
        protected Transform parent;
        public Transform Parent { get { return parent; } }

        [SerializeField]
        protected Transform anchor;
        public Transform Anchor { get { return anchor; } }

        [SerializeField]
        protected Vector3 position;
        public Vector3 Position { get { return position; } }

        [SerializeField]
        protected Vector3 angles;
        public Vector3 Angles { get { return angles; } }

        protected virtual void Reset()
        {
            parent = anchor = transform;
        }

        public override void Execute()
        {
            var instance = Instantiate(source);

            instance.transform.SetParent(anchor);
            instance.transform.localPosition = position;
            instance.transform.localEulerAngles = angles;

            instance.transform.SetParent(parent);
        }
    }
}