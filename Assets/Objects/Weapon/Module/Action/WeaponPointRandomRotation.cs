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
	public class WeaponPointRandomRotation : Weapon.Module
	{
        [SerializeField]
        protected Transform target;

        [SerializeField]
        protected float range = 2f;

        WeaponActionRate actionRate;
        public float Rate
        {
            get
            {
                if(enabled)
                    return actionRate.Value;

                return 0f;
            }
        }

        Vector3 offset;

        void Reset()
        {
            target = transform;
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            actionRate = weapon.FindModule<WeaponActionRate>();

            weapon.ProcessEvent += Process;
            weapon.LateProcessEvent += LateProcess;
        }

        void Process(bool input)
        {
            target.localEulerAngles -= offset;

            if (weapon.State == WeaponState.Idle || !enabled)
            {
                offset *= Rate;
            }
            else if(weapon.State == WeaponState.Action)
            {
                offset.x = Random.Range(-range, range);
                offset.y = Random.Range(-range, range);

                offset *= Rate;
            }
        }

        void LateProcess(bool input)
        {
            target.localEulerAngles += offset;
        }
    }
}