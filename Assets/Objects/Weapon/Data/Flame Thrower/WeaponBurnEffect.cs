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
	public class WeaponBurnEffect : Weapon.Module
	{
        [SerializeField]
        protected float value = 25;
        public float Value { get { return value; } }

        [SerializeField]
        protected WeaponBurnEffectMode mode = WeaponBurnEffectMode.PerShot;
        public WeaponBurnEffectMode Mode { get { return mode; } }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.Hit.OnInvoke += OnHit;
        }

        protected virtual void OnHit(WeaponHit.Data data)
        {
            if (data.Entity == null) return;
            if (data.Entity.Burn == null) return;

            switch (mode)
            {
                case WeaponBurnEffectMode.PerShot:
                    data.Entity.Burn.Apply(weapon.Owner, value);
                    break;

                case WeaponBurnEffectMode.PerSecond:
                    data.Entity.Burn.Apply(weapon.Owner, value * Time.deltaTime);
                    break;
            }
        }
    }

    public enum WeaponBurnEffectMode
    {
        PerShot, PerSecond
    }
}