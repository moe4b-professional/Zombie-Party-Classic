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
	public class WeaponHitEffects : Weapon.Module
	{
        [SerializeField]
        protected GameObject prefab;

        public void Instantiate(Collision collision)
        {
            var contact = collision.contacts.First();

            Instantiate(contact.point, contact.normal);
        }

        public void Instantiate(RaycastHit hit)
        {
            Instantiate(hit.point, hit.normal);
        }

        public void Instantiate(Vector3 position, Vector3 normal)
        {
            Instantiate(position, Quaternion.LookRotation(-normal));
        }

        public void Instantiate(Vector3 position, Quaternion rotation)
        {
            var instance = Instantiate(prefab);

            instance.transform.position = position;
            instance.transform.rotation = rotation;
        }
    }
}