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
    [DefaultExecutionOrder(-100)]
	public class MainMenu : MonoBehaviour
	{
		public static MainMenu Instance { get; protected set; }

        [SerializeField]
        protected Menu title;
        public Menu Title { get { return title; } }

        [SerializeField]
        protected StartMenu start;
        public StartMenu Initial { get { return start; } }

        [SerializeField]
        protected Popup popup;
        public Popup Popup { get { return popup; } }

        Core Core => Core.Asset;

        void Awake()
        {
            Instance = this;

            Time.timeScale = 1f;
        }

        void Start()
        {
            Core.UI.Container.Fade.Alpha = 1f;
            Core.UI.Container.Fade.Transition(0f);
        }
    }
}