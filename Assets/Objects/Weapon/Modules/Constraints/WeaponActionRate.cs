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
	public class WeaponActionRate : Weapon.Module
	{
        [SerializeField]
        protected float impact;

        [SerializeField]
        protected float relase;

        [SerializeField]
        protected float releaseDelay;
        float releaseTime = 0f;

        public float Value { get; protected set; }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ProcessEvent += Process;

            Value = 0f;
        }
        
        void Process(bool input)
        {
            if(weapon.State == WeaponState.Idle || !enabled)
            {
                releaseTime = Mathf.MoveTowards(releaseTime, 0f, Time.deltaTime);

                if (releaseTime == 0f)
                    Value = Mathf.MoveTowards(Value, 0f, relase * Time.deltaTime);
            }
            else if(weapon.State == WeaponState.Action)
            {
                releaseTime = releaseDelay;

                Value = Mathf.MoveTowards(Value, 1f, impact * Time.deltaTime);
            }
        }
    }
}