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
	public class WeaponMuzzleEffect : Weapon.Module
	{
        [SerializeField]
        protected GameObject target;

        [SerializeField]
        protected float duration = 0.0835989f;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += Action;

            target.SetActive(false);
        }

        void Action()
        {
            time = duration;

            target.SetActive(true);
        }

        float time = 0f;
        void Update()
        {
            if(time > 0)
            {
                time = Mathf.MoveTowards(time, 0f, Time.deltaTime);

                if (time == 0)
                    target.SetActive(false);
            }
        }
    }
}