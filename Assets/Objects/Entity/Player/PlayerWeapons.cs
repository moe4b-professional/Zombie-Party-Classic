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
	public class PlayerWeapons : MonoBehaviour, Player.IReference
	{
        Player player;

        public PlayerAim Aim { get { return player.Aim; } }

        public ObserverInput Input { get { return player.Observer.Input; } }

        public const float ShootMagnitude = 0.75f;

        [SerializeField]
        protected Weapon weapon;
        public Weapon Weapon { get { return weapon; } }

        public virtual void Init(Player reference)
        {
            this.player = reference;

            weapon.Init(player);
        }

        protected virtual void Update()
        {
            weapon.Process(Input.Look.magnitude > ShootMagnitude && Mathf.Abs(Aim.DeltaTargetAngle) < 15f);
        }
    }
}