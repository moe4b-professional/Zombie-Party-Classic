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
	public class WeaponParticleFX : Weapon.Module
	{
		[SerializeField]
        protected ParticleSystem particle;
        public ParticleSystem Particle { get { return particle; } }
        protected virtual void SetParticleEmission(bool value)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            particle.enableEmission = value;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        protected virtual void Reset()
        {
            particle = GetComponent<ParticleSystem>();
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            SetParticleEmission(false);

            weapon.ProcessEvent += OnProcess;
        }

        void OnProcess(bool input)
        {
            if(weapon.State == WeaponState.Action)
            {
                SetParticleEmission(true);
            }
            else
            {
                SetParticleEmission(false);
            }
        }
    }
}