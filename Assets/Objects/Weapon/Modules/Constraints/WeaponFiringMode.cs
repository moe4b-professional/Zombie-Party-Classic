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
	public class WeaponFiringMode : Weapon.Module, Weapon.IConstraint
	{
        bool active = false;
		public bool Active { get { return active; } }

        public FiringMode mode = FiringMode.Auto;
        public enum FiringMode
        {
            Auto, SemiAuto
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ProcessEvent += Process;

            weapon.ActionEvent += Action;
        }

        void Process(bool input)
        {
            if(active)
            {
                switch (mode)
                {
                    case FiringMode.Auto:
                        active = false;
                        break;

                    case FiringMode.SemiAuto:
                        if (!input)
                            active = false;
                        break;
                }
            }
        }

        void Action()
        {
            if (mode == FiringMode.SemiAuto)
                active = true;
        }
    }
}