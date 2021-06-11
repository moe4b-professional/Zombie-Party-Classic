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
	public class WeaponSoundAction : Weapon.Module
	{
        [SerializeField]
        protected AudioSource audioSource;

        [SerializeField]
        protected AudioClip clip;

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += Action;
        }

        void Action()
        {
            if (!enabled) return;

            audioSource.PlayOneShot(clip);
        }
    }
}