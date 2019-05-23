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
    public class PlayerSprint : MonoBehaviour, IPlayerReference
    {
        public float DotProduct { get; protected set; }

        public float DotProductRate { get; protected set; }


        /// <summary>
        /// Linear Value Repreasenting How Much Sprint Is Happen, Ranges Between 0 and 1
        /// </summary>
        public float Rate { get; protected set; }

        public float MinValue { get { return 1f; } }

        [SerializeField]
        protected float maxMultiplier = 2f;
        public float MaxMultiplier { get { return maxMultiplier; } }

        /// <summary>
        /// Linear Value Repreasenting How Much Sprint Is Happen, Ranges Between MinMultiplier (1f by default) and MaxMultiplier
        /// </summary>
        public float Multiplier { get; protected set; }


        Player player;

        new public Rigidbody rigidbody { get { return player.rigidbody; } }

        public ObserverInput Input { get { return player.Input; } }

        public PlayerBody Body { get { return player.Body; } }

        public Animator Animator { get { return Body.Animator; } }

        public void Init(Player reference)
        {
            this.player = reference;
        }

        public virtual void Process()
        {
            var direction = Vector3.forward * Input.Move.y + Vector3.right * Input.Move.x;

            DotProduct = Vector3.Dot(player.transform.forward, direction);

            Rate = Mathf.InverseLerp(-1f, 1f, DotProduct);

            Rate = Mathf.Lerp(Rate, 0f, Input.Look.magnitude * 2f);

            Multiplier = Mathf.Lerp(MinValue, maxMultiplier, Rate);

            Animator.SetFloat("Sprint", Multiplier);
        }
    }
}