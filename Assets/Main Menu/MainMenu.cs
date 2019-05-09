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
    [DefaultExecutionOrder(-100)]
	public class MainMenu : MonoBehaviour
	{
		public static MainMenu Instance { get; protected set; }

        [SerializeField]
        protected Menu title;
        public Menu Title { get { return title; } }

        [SerializeField]
        protected ServerMainMenu server;
        public ServerMainMenu Server { get { return server; } }

        [SerializeField]
        protected ClientMainMenu client;
        public ClientMainMenu Client { get { return client; } }

        [SerializeField]
        protected Popup popup;
        public Popup Popup { get { return popup; } }

        void Awake()
        {
            Instance = this;
        }
	}
}