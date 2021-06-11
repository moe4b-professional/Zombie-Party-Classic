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
	public class WeaponRPM : Weapon.Module, Weapon.IConstraint
	{
        [SerializeField]
        uint value = 800;
        public uint Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public float Delay
        {
            get
            {
                return 60f / value;
            }
        }

        float time = 0f;

        public bool Active { get { return time > 0f; } }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ProcessEvent += Process;
            weapon.ActionEvent += Action;
        }

        void Process(bool input)
        {
            time = Mathf.MoveTowards(time, 0f, Time.deltaTime);
        }

        void Action()
        {
            if (!enabled) return;

            time = Delay;
        }
    }
}