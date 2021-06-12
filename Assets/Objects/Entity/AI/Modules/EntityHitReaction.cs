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
	public class EntityHitReaction : MonoBehaviour, IEntityReference
    {
        [SerializeField]
        int range = 2;

        Entity Entity;
        public void Init(Entity reference)
        {
            Entity = reference;

            Animator = Dependancy.Get<Animator>(Entity.gameObject);
        }

        public Animator Animator { get; protected set; }

        void Start()
        {
            Entity.OnTookDamage += TookDamageCallback;
        }

        void TookDamageCallback(Entity damager, float value)
        {
            Animator.SetInteger("Hit Type", Random.Range(1, range + 1));
            Animator.SetTrigger("Hit");
        }
    }
}