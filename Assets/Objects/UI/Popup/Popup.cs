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
    [RequireComponent(typeof(RectTransform))]
    public class Popup : UIElement
    {
        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }
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

        [SerializeField]
        protected LabeledButton button;
        public LabeledButton Button { get { return button; } }
        public bool Interactable
        {
            get
            {
                return button.gameObject.activeInHierarchy;
            }
            set
            {
                Button.gameObject.SetActive(value);
            }
        }

        public override void Init()
        {
            base.Init();

            button.onClick.AddListener(onClick);
        }

        public void Show(string text)
        {
            Transition.Value = 0f;

            this.Text = text;

            Interactable = false;

            Show();
        }
        public void Show(string text, Action action, string buttonText)
        {
            Transition.Value = 0f;

            this.Text = text;

            if (action == null) throw new NullReferenceException();

            this.Interactable = true;
            this.action = action;

            this.button.Text = buttonText;

            Show();
        }

        Action action;
        public virtual void onClick()
        {
            if (action != null) action();
        }
    }
}