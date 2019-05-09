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
    public class WeaponProjectileAction : Weapon.Module
    {
        [SerializeField]
        protected GameObject prefab;

        [SerializeField]
        protected Transform point;

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
            var instance = Instantiate(prefab, point.position, point.rotation);

            var projectile = instance.GetComponent<Projectile>();

            projectile.Init(weapon);
        }
    }
}