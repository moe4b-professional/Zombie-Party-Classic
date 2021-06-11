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
	public class WeaponAmmo : Weapon.Module, Weapon.IConstraint
	{
        public bool Active { get { return clip == 0f; } }

        [SerializeField]
        uint clip;
        public uint Clip { get { return clip; } }

        [SerializeField]
        uint clipSize = 30;

        [SerializeField]
        uint consumption = 1;
        public event Action OnConsumption;
        protected virtual void Consume()
        {
            clip -= consumption;

            if (OnConsumption != null) OnConsumption();
        }

        [SerializeField]
        uint reserve;

        [SerializeField]
        uint maxReserve = 120;

        void Reset()
        {
            clip = clipSize;

            reserve = maxReserve;
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += Action;
        }

        private void Action()
        {
            if (!enabled) return;

            Consume();
        }

        public bool CanRefil
        {
            get
            {
                return clip != clipSize && reserve > 0;
            }
        }

        public virtual void Refil()
        {
            if (reserve == 0) return;

            var requiredAmmo = clipSize - clip;

            if(requiredAmmo > reserve)
            {
                clip += reserve;
                reserve = 0;
            }
            else
            {
                clip += requiredAmmo;
                reserve -= requiredAmmo;
            }
        }
    }
}