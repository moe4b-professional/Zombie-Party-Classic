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
	public class Projectile : MonoBehaviour
	{
        [SerializeField]
        float damage = 50f;

        Weapon weapon;

        WeaponHitEffects hitEffect;

        public virtual void Init(Weapon weapon)
        {
            this.weapon = weapon;

            hitEffect = weapon.GetComponentInChildren<WeaponHitEffects>();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (hitEffect != null)
                hitEffect.Instantiate(collision);

            var entity = collision.gameObject.GetComponent<Entity>();

            if(entity != null)
                weapon.Damage(entity, damage);
        }
	}
}