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
    [CreateAssetMenu(menuName = MenuPath + "UI")]
	public class UICore : Core.Module
	{
		[SerializeField]
		GameObject prefab;

        public UIContainer Container { get; protected set; }

        public ScreenFade Fade => Container.Fade;
        public Popup Popup => Container.Popup;

        public override void Init()
        {
            base.Init();

            Container = Instantiate(prefab).GetComponent<UIContainer>();
            DontDestroyOnLoad(Container.gameObject);
        }
    }
}