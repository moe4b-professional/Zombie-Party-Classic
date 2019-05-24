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
	public class PlayerMovement : MonoBehaviour, IPlayerReference
	{
        [SerializeField]
        protected float speed;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float acceleration;
        public float Acceleration { get { return acceleration; } }

        public PlayerSprint Sprint { get; protected set; }

        Player player;

#pragma warning disable CS0109
        new public Rigidbody rigidbody { get { return player.rigidbody; } }
#pragma warning restore CS0109

        public ObserverInput Input { get { return player.Input; } }

        public PlayerBody Body { get { return player.Body; } }

        public Animator Animator { get { return Body.Animator; } }

        public void Init(Player reference)
        {
            this.player = reference;

            Sprint = Dependancy.Get<PlayerSprint>(player.gameObject);
        }

        void Update()
        {
            Sprint.Process();

            var target = Vector3.forward * Input.Move.y +
                Vector3.right * Input.Move.x;

            target = Vector3.ClampMagnitude(target, 1f);

            target *= speed * Sprint.Multiplier;
            target.y = rigidbody.velocity.y;

            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, target, acceleration * Time.deltaTime);

            Animate();
        }

        void Animate()
        {
            var velocity = rigidbody.velocity / (speed * Sprint.Multiplier);

            velocity = transform.InverseTransformDirection(velocity);

            Animator.SetFloat("Vertical", velocity.z);
            Animator.SetFloat("Horizontal", velocity.x);
        }
	}
}