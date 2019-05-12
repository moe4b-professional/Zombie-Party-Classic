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
    [RequireComponent(typeof(Animator))]
    public class PopupLabel : MonoBehaviour, Initializer.Interface
    {
        [SerializeField]
        Text label;
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

        Animator animator;

        public void Init()
        {
            animator = GetComponent<Animator>();
        }

        public void Show(string text)
        {
            this.Text = text;

            Show();
        }

        public void Show()
        {
            StopAllCoroutines();

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
	}
}