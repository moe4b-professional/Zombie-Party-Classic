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
    [RequireComponent(typeof(RectTransform))]
    public class Popup : UIElement
    {
        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

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

        void OnEnable()
        {
            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            yield return new WaitForEndOfFrame();

            UpdateLayout();
        }

        void UpdateLayout()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(0) as RectTransform);
        }
	}
}