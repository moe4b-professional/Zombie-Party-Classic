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
	public class UIElement : MonoBehaviour
	{
		public bool Visible
        {
            get
            {
                return gameObject.activeSelf;
            }
            set
            {
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public event Action OnShow;
        public virtual void Show()
        {
            gameObject.SetActive(true);

            if (OnShow != null) OnShow();
        }

        public event Action OnHide;
        public virtual void Hide()
        {
            gameObject.SetActive(false);

            if (OnHide != null) OnHide();
        }
    }
}