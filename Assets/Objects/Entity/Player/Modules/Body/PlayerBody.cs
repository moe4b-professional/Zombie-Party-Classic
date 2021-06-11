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
    [RequireComponent(typeof(Animator))]
	public class PlayerBody : MonoBehaviour, IPlayerReference
	{
        public Animator Animator { get; protected set; }

        public IKController RightHandIK { get; protected set; }
        public IKController LeftHandIK { get; protected set; }
        [Serializable]
        public class IKController
        {
            public Animator Animator { get; protected set; }

            public AvatarIKGoal IKGoal { get; protected set; }

            public HumanBodyBones Bone { get; protected set; }
            public Transform BoneTransform { get; protected set; }

            public static HumanBodyBones IKGoalToBone(AvatarIKGoal IKGoal)
            {
                switch (IKGoal)
                {
                    case AvatarIKGoal.LeftFoot:
                        return HumanBodyBones.LeftFoot;

                    case AvatarIKGoal.RightFoot:
                        return HumanBodyBones.RightFoot;

                    case AvatarIKGoal.LeftHand:
                        return HumanBodyBones.LeftHand;

                    case AvatarIKGoal.RightHand:
                        return HumanBodyBones.RightHand;
                }

                throw new NotImplementedException();
            }

            float _weight;
            public float Weight
            {
                get
                {
                    return _weight;
                }
                set
                {
                    value = Mathf.Clamp01(value);

                    _weight = value;
                }
            }

            Vector3 _position;
            public Vector3 Position
            {
                get
                {
                    return _position;
                }
                set
                {
                    _position = value;
                }
            }

            public virtual void Process()
            {
                Animator.SetIKPositionWeight(IKGoal, _weight);

                Animator.SetIKPosition(IKGoal, _position);
            }

            public IKController(Animator animator, AvatarIKGoal IKGoal)
            {
                this.Animator = animator;
                this.IKGoal = IKGoal;

                Bone = IKGoalToBone(IKGoal);
                BoneTransform = animator.GetBoneTransform(Bone);

                Weight = 0f;
                Position = Vector3.zero;
            }
        }

        Player player;
		public virtual void Init(Player reference)
        {
            player = reference;

            Animator = GetComponent<Animator>();

            RightHandIK = new IKController(Animator, AvatarIKGoal.RightHand);
            LeftHandIK = new IKController(Animator, AvatarIKGoal.LeftHand);
        }

        protected virtual void OnAnimatorMove()
        {

        }

        protected virtual void OnAnimatorIK()
        {
            RightHandIK.Process();
            LeftHandIK.Process();
        }
	}
}