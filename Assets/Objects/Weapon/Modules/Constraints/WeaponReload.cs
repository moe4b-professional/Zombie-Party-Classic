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
	public class WeaponReload : Weapon.Module, Weapon.IConstraint
	{
        [SerializeField]
        protected Animator animator;

        [SerializeField]
        protected string trigger = "Reload";

        WeaponAmmo ammo;

        bool active = false;
        public bool Active { get { return active; } }

        [SerializeField]
        bool autoReload = true;

        [SerializeField]
        float autoReloadDelay = 0.4f;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            if (weapon.Model == null)
                throw new MissingComponentException("A " + typeof(WeaponModel).Name + " needs to be attached to this weapon for the " + GetType().Name + " to work");

            weapon.Model.AnimationTriggers.Add(End, "Reload End");


            ammo = weapon.FindModule<WeaponAmmo>();

            if(ammo == null)
                throw new MissingComponentException("A " + typeof(WeaponAmmo).Name + " needs to be attached to this weapon for the " + GetType().Name + " to work");

            ammo.OnConsumption += AmmoConsumed;
        }

        void AmmoConsumed()
        {
            if(autoReload && ammo.Clip == 0)
                autoReloadCoroutine = StartCoroutine(AutoReloadProcedure());
        }

        Coroutine autoReloadCoroutine;
        IEnumerator AutoReloadProcedure()
        {
            yield return new WaitForSeconds(autoReloadDelay);

            Begin();
        }

        public void Begin()
        {
            if (!ammo.CanRefil) return;

            if(autoReloadCoroutine != null)
            {
                StopCoroutine(autoReloadCoroutine);
                autoReloadCoroutine = null;
            }

            active = true;

            animator.SetTrigger(trigger);
        }

        void End()
        {
            active = false;

            ammo.Refil();
        }
    }
}