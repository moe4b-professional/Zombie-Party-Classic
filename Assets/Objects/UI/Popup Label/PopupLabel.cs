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
    public class PopupLabel : UIElement
    {
        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected float displayDuration = 2.5f;
        public float DisplayDuration { get { return displayDuration; } }

        public string Text
        {
            get
            {
                return label.text;
            }
            set
            {
                label.text = value;
            }
        }

        public void Show(string text)
        {
            this.Text = text;

            Show();
        }

        public override void Show()
        {
            base.Show();

            Invoke("Hide", displayDuration);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public static string Colorize(string value, string color)
        {
            return "<color=" + color + ">" + value + "</color>";
        }
	}
}