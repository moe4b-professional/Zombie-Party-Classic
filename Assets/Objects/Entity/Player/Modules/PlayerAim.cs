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
	public class PlayerAim : MonoBehaviour, IPlayerReference
	{
        [SerializeField]
        protected float speed;
        public float Speed { get { return speed; } }

        [SerializeField]
        AssistData assist;
        [Serializable]
        public class AssistData
        {
            [SerializeField]
            float distance = 10;
            public float Distance => distance;

            [SerializeField]
            float angle = 45;
            public float Angle => angle;
        }

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
            if (Input.Look.magnitude > 0f)
                Process();
            else if (Input.Move.magnitude > 0f)
                LookTowards(Vector2Angle(Input.Move));
        }

        void Process()
        {
            var direction = new Vector3(Input.Look.x, 0f, Input.Look.y);

            var target = AI.Query.Find(player.transform.position, direction, assist.Distance, assist.Angle);

            if (target.AI == null)
                LookTowards(Vector2Angle(Input.Look));
            else
                LookTowards(Vector2Angle(target.Direction));
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

        public static float Vector2Angle(Vector3 vector3) => Vector2Angle(vector3.x, vector3.z);
        public static float Vector2Angle(Vector2 vector2) => Vector2Angle(vector2.x, vector2.y);
        public static float Vector2Angle(float x, float y)
        {
            return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        }
    }
}