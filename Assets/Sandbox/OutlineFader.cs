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
	public class OutlineFader : MonoBehaviour
	{
        public void Begin(GameObject target)
        {
            StartCoroutine(Process(target));
        }
        IEnumerator Process(GameObject target)
        {
            var renderer = target.GetComponentInChildren<Renderer>(true);

            var power = renderer.material.GetFloat("_Power");

            var block = new MaterialPropertyBlock();

            while (true)
            {
                power = Mathf.MoveTowards(power, 0f, 0.5f * Time.deltaTime);
                block.SetFloat("_Power", power);

                renderer.SetPropertyBlock(block);

                if (power == 0) break;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}