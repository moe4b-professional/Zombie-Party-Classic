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
	public class WeaponKick : Weapon.Module
	{
        [SerializeField]
        protected FloatAxisData multiplier = new FloatAxisData(40f, 10f);

        [SerializeField]
        public Player player;

        WeaponActionRate actionRate;
        public float Rate
        {
            get
            {
                if (enabled)
                    return actionRate.Value;

                return 0f;
            }
        }

        Vector3 offset;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            actionRate = weapon.FindModule<WeaponActionRate>();

            weapon.ProcessEvent += Process;
            
            weapon.LateProcessEvent += LateProcess;
        }

        void Process(bool input)
        {
            if (weapon.State == WeaponState.Idle || !enabled)
            {
                offset = Vector3.zero;
            }
            else if(weapon.State == WeaponState.Action)
            {
                offset.x = Random.Range(-multiplier.Vertical, 0f);
                offset.y = Random.Range(-multiplier.Horizontal, multiplier.Horizontal);

                offset *= Rate * Time.deltaTime;
            }
        }

        void LateProcess(bool input)
        {
        }
    }
}