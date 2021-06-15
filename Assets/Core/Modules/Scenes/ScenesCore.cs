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
    [CreateAssetMenu(menuName = MenuPath + "Levels")]
	public class ScenesCore : Core.Module
	{
        [SerializeField]
        protected MSceneAsset mainMenu;
        public MSceneAsset MainMenu { get { return mainMenu; } }

        [SerializeField]
        protected MSceneAsset level;
        public MSceneAsset Level { get { return level; } }

        public virtual void Load(MSceneAsset asset) => Load(asset.ID);
        public virtual void Load(string name)
        {
            Core.SceneAccessor.StartCoroutine(Procedure());
            IEnumerator Procedure()
            {
                yield return Core.UI.Container.Fade.Transition(1f);

                yield return new WaitForSecondsRealtime(1f);

                yield return SceneManager.LoadSceneAsync(name);

                yield return new WaitForSecondsRealtime(1f);

                yield return Core.UI.Container.Fade.Transition(0f);
            }
        }
    }
}