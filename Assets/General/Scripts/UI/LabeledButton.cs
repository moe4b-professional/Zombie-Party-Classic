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
	public class LabeledButton : Button, Initializer.Interface
	{
        public Text Label { get; protected set; }

        public string Text
        {
            get
            {
                return Label.text;
            }
            set
            {
                Label.text = value;

                LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            }
        }

        public virtual void Init()
        {
            Label = Dependancy.Get<Text>(gameObject);
        }
    }
}