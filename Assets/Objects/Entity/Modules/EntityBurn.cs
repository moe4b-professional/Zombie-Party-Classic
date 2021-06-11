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
	public class EntityBurn : MonoBehaviour, IEntityReference
	{
        [SerializeField]
        [Range(0f, MaxValue)]
        protected float value;
        public float Value { get { return value; } }

        public const float MaxValue = 100;

        [SerializeField]
        [Range(0f, 1f)]
        protected float resistance;
        public float Resistance { get { return resistance; } }

        [SerializeField]
        protected float recovery = 2f;
        public float Recovery { get { return recovery; } }

        [SerializeField]
        protected float damage = 10;
        public float Damage { get { return damage; } }

        [SerializeField]
        protected ParticleSystem particle;
        public ParticleSystem Particle { get { return particle; } }
        protected virtual void SetParticleEmission(bool value)
        {
            if (value) Particle.Play(true);
            else particle.Stop(true);

#pragma warning disable CS0618 // Type or member is obsolete
            particle.enableEmission = value;
#pragma warning restore CS0618 // Type or member is obsolete
        }

#pragma warning disable CS0109
        new SkinnedMeshRenderer renderer;
#pragma warning restore CS0109

        public virtual void SetModel(GameObject gameObject)
        {
            renderer = gameObject.GetComponent<SkinnedMeshRenderer>();

            var shape = particle.shape;
            shape.skinnedMeshRenderer = renderer;
        }

        [SerializeField]
        protected bool colorize = true;
        public bool Colorize { get { return colorize; } }

        Entity entity;
		public virtual void Init(Entity reference)
        {
            this.entity = reference;

            if(renderer == null)
                renderer = entity.GetComponentInChildren<SkinnedMeshRenderer>();

            SetParticleEmission(false);
        }

        public virtual void Apply(Entity damager, float increase)
        {
            value += Mathf.Lerp(increase, 0f, resistance);

            if (value >= MaxValue)
            {
                value = MaxValue;

                if (coroutine == null)
                    coroutine = StartCoroutine(Procedure(damager));
            }

            if (colorize)
                renderer.material.color = Color.Lerp(Color.white, Color.black, value / MaxValue * 0.85f);
        }

        Coroutine coroutine;
        public virtual bool Active { get { return coroutine != null; } }
        protected virtual IEnumerator Procedure(Entity damager)
        {
            SetParticleEmission(true);

            while (value > 0f)
            {
                entity.TakeDamage(damager, damage * Time.deltaTime);

                value = Mathf.MoveTowards(value, 0f, recovery * Time.deltaTime);

                yield return null;
            }

            SetParticleEmission(false);

            coroutine = null;
        }
	}
}