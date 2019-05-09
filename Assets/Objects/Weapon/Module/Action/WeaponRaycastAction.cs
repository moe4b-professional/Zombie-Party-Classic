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
    public class WeaponRaycastAction : Weapon.Module
    {
        [SerializeField]
        LayerMask mask = Physics.DefaultRaycastLayers;

        [SerializeField]
        float range = 400f;

        [SerializeField]
        float damage = 50f;

        [SerializeField]
        QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

        [SerializeField]
        Transform point;

        WeaponHitEffects hitEffect;

        void Reset()
        {
            point = transform;
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += Action;

            hitEffect = weapon.GetComponentInChildren<WeaponHitEffects>();
        }

        public event Action<RaycastHit> OnHit;
        void Action()
        {
            if (!enabled) return;

            RaycastHit hit;

            if(Physics.Raycast(point.position, point.forward, out hit, range, mask, triggerInteraction))
            {
                if(hitEffect != null)
                    hitEffect.Instantiate(hit);

                var entity = hit.transform.GetComponent<Entity>();

                if (entity != null)
                    weapon.Damage(entity, damage);

                if (OnHit != null) OnHit(hit);

            }
            else
            {

            }
        }
    }
}