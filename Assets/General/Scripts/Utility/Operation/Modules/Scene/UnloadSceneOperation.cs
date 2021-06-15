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

using MB;

namespace Default
{
	public class UnloadSceneOperation : Operation.Behaviour
	{
        public MSceneAsset target;

        public override void Execute()
        {
            StartCoroutine(Procedure());
        }

        protected virtual IEnumerator Procedure()
        {
            var operation = SceneManager.UnloadSceneAsync(target);

            operation.allowSceneActivation = true;

            yield return operation;
        }
    }
}