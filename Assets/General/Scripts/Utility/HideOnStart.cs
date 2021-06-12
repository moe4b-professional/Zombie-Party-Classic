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
	public class HideOnStart : MonoBehaviour
	{
        void Start()
        {
            var renderers = GetComponentsInChildren<Renderer>(true);

            foreach (var item in renderers)
                item.enabled = false;
        }
    }
}