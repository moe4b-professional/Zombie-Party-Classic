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
	public class UIContainer : MonoBehaviour
	{
        [SerializeField]
        ScreenFade fade = default;
        public ScreenFade Fade => fade;

        [SerializeField]
        Popup popup = default;
        public Popup Popup => popup;

        void Awake()
        {
            Initializer.Perform(this);
        }
    }
}