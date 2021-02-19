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
	public class WeaponHit : Weapon.Module
	{
        public event InvokeDelgate OnInvoke;
        public delegate void InvokeDelgate(Data data);

        public struct Data
        {
            public GameObject GameObject { get; private set; }
            public Entity Entity { get; private set; }

            public Vector3 Point { get; private set; }

            public Data(GameObject gameObject, Entity entity, Vector3 position)
            {
                this.GameObject = gameObject;
                this.Entity = entity;
                this.Point = position;
            }
        }

        public virtual void Invoke(Data data)
        {
            if (OnInvoke != null) OnInvoke(data);
        }

        public virtual void Invoke(GameObject gameObject, Entity entity, Vector3 position)
        {
            var data = new Data(gameObject, entity, position);

            Invoke(data);
        }
    }
}