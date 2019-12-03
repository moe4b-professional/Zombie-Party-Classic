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

using UnityEngine.Rendering;

namespace Game
{
	public class AutoTwoSidedShadows : MonoBehaviour
	{
		void Awake()
        {
            Apply(gameObject);

            Destroy(this);
        }

        public static void Apply(GameObject gameObject)
        {
            foreach (var renderer in Dependancy.GetAll<Renderer>(gameObject))
            {
                if(renderer.shadowCastingMode == ShadowCastingMode.On)
                    renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            }
        }
	}
}