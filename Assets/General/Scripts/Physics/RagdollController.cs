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
	public class RagdollController : MonoBehaviour
	{
		public IList<Rigidbody> rigidbodies { get; protected set; }
		public IList<Collider> colliders { get; protected set; }

        public virtual void Enable()
        {
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                rigidbodies[i].isKinematic = false;
                colliders[i].enabled = true;
            }
        }

        public virtual void Disable()
        {
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                rigidbodies[i].isKinematic = true;
                colliders[i].enabled = false;
            }
        }

        protected virtual void Awake()
        {
            rigidbodies = Dependancy.GetAll<Rigidbody>(gameObject);
            colliders = Dependancy.GetAll<Collider>(gameObject);

            Disable();
        }

        public virtual void Fallout(float delay)
        {
            Enable();

            StartCoroutine(FalloutProcedure(delay));
        }
        protected virtual IEnumerator FalloutProcedure(float delay)
        {
            yield return new WaitForSeconds(delay);

            for (int i = 0; i < colliders.Count; i++)
                colliders[i].isTrigger = true;

            yield return new WaitForSeconds(4f);

            Destroy(gameObject);
        }
	}
}