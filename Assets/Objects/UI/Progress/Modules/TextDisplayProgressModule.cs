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
    [RequireComponent(typeof(Text))]
	public class TextDisplayProgressModule : ProgressBar.Module
	{
        [SerializeField]
        [HideInInspector]
        Text label;

        [SerializeField]
        protected float multiplier = 100f;
        public float Multiplier { get { return multiplier; } }

        [SerializeField]
        protected string suffix = "%";
        public string Suffix { get { return suffix; } }

        protected override void GetDependancies()
        {
            base.GetDependancies();

            label = GetComponent<Text>();
        }

        protected override void SetValue(float value)
        {
            base.SetValue(value);

            label.text = (Math.Round(value * multiplier, 1)).ToString() + suffix;
        }
    }
}