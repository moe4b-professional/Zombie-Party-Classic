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
	public class WeaponContinuousSound : Weapon.Module
	{
        [SerializeField]
        protected AudioSource audioSource;
        public AudioSource AudioSource { get { return audioSource; } }

        [SerializeField]
        protected AudioClip clip;
        public AudioClip Clip { get { return clip; } }

        [SerializeField]
        protected float delta = 5;
        public float Delta { get { return delta; } }

        public float Value
        {
            get
            {
                return audioSource.volume;
            }
            set
            {
                audioSource.volume = value;
            }
        }

        public float MaxValue { get; protected set; }

        public float Target { get; protected set; }

        protected virtual void Reset()
        {
            audioSource = Dependancy.Get<AudioSource>(gameObject, Dependancy.Scope.RecursiveToParents);
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ProcessEvent += Process;

            MaxValue = audioSource.volume;
            Value = 0f;

            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }

        void Process(bool input)
        {
            if(weapon.State == WeaponState.Action)
            {
                Target = MaxValue;
            }

            if(weapon.State == WeaponState.Idle)
            {
                Target = 0f;
            }

            Value = Mathf.MoveTowards(Value, Target, delta * Time.deltaTime);
        }
    }
}