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
	public class WeaponRotatingBarrelBasedRPM : Weapon.Module
	{
        WeaponRotatingBarrel rotatingBarrel;

        WeaponRPM RPM;

        [SerializeField]
        protected uint max = 2000;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            rotatingBarrel = weapon.FindModule<WeaponRotatingBarrel>();

            RPM = weapon.FindModule<WeaponRPM>();

            weapon.ProcessEvent += Process;
        }

        void Process(bool input)
        {
            RPM.Value = (uint)Mathf.Lerp(0, max, rotatingBarrel.Rate);
        }
    }
}