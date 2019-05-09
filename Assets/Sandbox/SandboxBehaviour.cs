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
	public class SandboxBehaviour : MonoBehaviour
	{
        Animator animator;

        new Camera camera;

        new Rigidbody rigidbody;

        public float speed = 5f;

        public float acceleration = 15f;

        public IKController RightHand { get; protected set; }
        public IKController LeftHand { get; protected set; }
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

        public Transform rightHandTarget;
        public Transform leftHandTarget;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            animator = GetComponent<Animator>();

            camera = FindObjectOfType<Camera>();

            RightHand = new IKController(animator, AvatarIKGoal.RightHand);
            LeftHand = new IKController(animator, AvatarIKGoal.LeftHand);
        }

        void Update()
        {
            Look();

            Move();

            Animate();
        }

        void Move()
        {
            var input = new Vector2()
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };

            var targetVelocity = Vector3.forward * input.y + Vector3.right * input.x;
            targetVelocity *= speed;

            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, targetVelocity, acceleration * Time.deltaTime);

            Animate();
        }

        void Animate()
        {
            var velocity = rigidbody.velocity / speed;

            velocity *= 2f;

            velocity = transform.InverseTransformDirection(velocity);

            animator.SetFloat("Vertical", velocity.z);
            animator.SetFloat("Horizontal", velocity.x);
        }

        void OnAnimatorIK()
        {
            RightHand.Weight = 1f;
            RightHand.Position = rightHandTarget.position;

            LeftHand.Weight = 1f;
            LeftHand.Position = leftHandTarget.position;

            RightHand.Process();
            LeftHand.Process();
        }

        void Look()
        {
            var direction = Vector3.zero;

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                direction = (hit.point - transform.position).normalized;
                direction.y = direction.z;

                Debug.DrawLine(camera.transform.position, hit.point);
            }
            else
            {

            }

            var angles = transform.eulerAngles;

            angles.y = Vector2Angle(direction);

            transform.eulerAngles = angles;
        }

        public static float Vector2Angle(Vector2 vector2)
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }
}