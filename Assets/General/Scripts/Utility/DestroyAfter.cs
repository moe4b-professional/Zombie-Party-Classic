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
	public class DestroyAfter : MonoBehaviour
	{
        protected float duration = 4f;
        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (value < 0f) value = 0f;

                duration = value;
            }
        }

        IEnumerator Start()
        {
            var time = duration;

            while(time > 0f)
            {
                time = Mathf.MoveTowards(time, 0f, Time.deltaTime);

                yield return null;
            }

            Destroy(gameObject);
        }
	}
}