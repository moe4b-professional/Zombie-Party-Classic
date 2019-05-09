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
	public class ToggleTextSet : ToggleSet<string>
    {
        Text label;

        protected override void Awake()
        {
            base.Awake();

            label = GetComponent<Text>();
        }

        protected override void Set(string value)
        {
            label.text = value;
        }
    }
}