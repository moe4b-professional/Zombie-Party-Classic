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
	public class WeaponAnimationTriggerAction : Weapon.Module
	{
        [SerializeField]
        protected Animator animator;

        [SerializeField]
        protected string trigger;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += Action;
        }

        void Action()
        {
            if (!enabled) return;

            animator.SetTrigger(trigger);
        }
    }
}