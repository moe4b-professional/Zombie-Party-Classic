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

using UnityEngine.Networking;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        public EntityHealth Health { get; protected set; }

        public virtual bool IsAlive { get { return Health.Value > 0f; } }
        public virtual bool IsDead { get { return Health.Value == 0f; } }

        protected virtual void Awake()
        {
            Health = GetComponent<EntityHealth>();
        }

        protected virtual void Start()
        {

        }
    }
}