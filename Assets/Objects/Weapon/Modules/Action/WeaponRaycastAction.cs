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

        void Reset()
        {
            point = transform;
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += Action;
        }

        void Action()
        {
            if (!enabled) return;

            RaycastHit raycastHit;

            if(Physics.Raycast(point.position, point.forward, out raycastHit, range, mask, triggerInteraction))
            {
                var entity = raycastHit.transform.GetComponent<Entity>();

                if (entity != null)
                    weapon.Damage(entity, damage);

                weapon.Hit.Invoke(raycastHit.transform.gameObject, entity, raycastHit.point);
            }
            else
            {

            }
        }
    }
}