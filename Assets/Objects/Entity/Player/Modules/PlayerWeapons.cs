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
	public class PlayerWeapons : MonoBehaviour, IPlayerReference
	{
        Player player;

        public PlayerAim Aim { get { return player.Aim; } }

        public ObserverInput Input { get { return player.Observer.Input; } }

        public const float ShootMagnitude = 0.75f;

        public Weapon Weapon { get; protected set; }

        public virtual void Init(Player reference)
        {
            this.player = reference;
        }

        public virtual void Set(Weapon weapon)
        {
            this.Weapon = weapon;

            this.Weapon.Init(player);
        }

        protected virtual void Update()
        {
            if(Weapon != null)
                Weapon.Process(Input.Look.magnitude > ShootMagnitude && Mathf.Abs(Aim.DeltaTargetAngle) < 15f);
        }
    }
}