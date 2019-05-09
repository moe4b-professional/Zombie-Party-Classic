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
	public class PlayerAim : MonoBehaviour, Player.IReference
	{
        [SerializeField]
        protected float speed;
        public float Speed { get { return speed; } }

        Player player;

        PlayerMovement Movement { get { return player.Movement; } }
        public PlayerSprint Sprint { get { return Movement.Sprint; } }

        public ObserverInput Input { get { return player.Input; } }

        public PlayerBody Body { get { return player.Body; } }

        public Animator Animator { get { return Body.Animator; } }

        public void Init(Player reference)
        {
            this.player = reference;
        }

        void Update()
        {
            if(Input.Look.magnitude > 0f)
                LookTowards(Vector2Angle(Input.Look));
            else if(Input.Move.magnitude > 0f)
                LookTowards(Vector2Angle(Input.Move));
        }

        Vector3 angles;
        public float DeltaTargetAngle { get; protected set; }
        void LookTowards(float yAngle)
        {
            angles = player.transform.eulerAngles;

            var sprintMultipluer = Mathf.Lerp(1f, 4f, Sprint.Rate);

            angles.y = Mathf.MoveTowardsAngle(angles.y, yAngle, speed / sprintMultipluer * Time.deltaTime);

            DeltaTargetAngle = Mathf.DeltaAngle(angles.y, yAngle);

            player.transform.eulerAngles = angles;
        }

        public static float Vector2Angle(Vector2 vector2)
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }
}