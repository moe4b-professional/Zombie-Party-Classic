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
    [RequireComponent(typeof(CanvasGroup))]
	public class UITransitionFade : UITransition.Module
	{
        public CanvasGroup CanvasGroup { get; protected set; }

        public override void Init()
        {
            CanvasGroup = GetComponent<CanvasGroup>();

            base.Init();

            transition.OnValueChanged += OnValueChanged;
        }

        protected override void UpdateState(float value)
        {
            base.UpdateState(value);

            CanvasGroup.alpha = value;
        }
    }
}