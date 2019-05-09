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
	public class PlayerHandsIKTargets : MonoBehaviour, Player.IReference
	{
        [SerializeField]
        protected TargetData right;
        public TargetData Right { get { return right; } }

        [SerializeField]
        protected TargetData left;
        public TargetData Left { get { return left; } }

        [Serializable]
        public class TargetData
        {
            [SerializeField]
            protected Transform transform;
            public Transform Transform { get { return transform; } }

            [SerializeField]
            [Range(0f, 1f)]
            protected float weight;
            public float Weight { get { return weight; } }
        }

        PlayerBody body;
        public virtual void Init(Player reference)
        {
            body = reference.Body;
        }

        void Update()
        {
            body.RightHandIK.Position = right.Transform.position;
            body.RightHandIK.Weight = right.Weight;

            body.LeftHandIK.Position = left.Transform.position;
            body.LeftHandIK.Weight = left.Weight;
        }
	}
}