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
    [CreateAssetMenu(menuName = MenuPath + "Levels")]
	public class ScenesCore : Core.Module
	{
        [SerializeField]
        protected GameScene mainMenu;
        public GameScene MainMenu { get { return mainMenu; } }

        [SerializeField]
        protected GameScene level;
        public GameScene Level { get { return level; } }

        [SerializeField]
        protected GameScene client;
        public GameScene Client { get { return client; } }

        [SerializeField]
        protected GameScene server;
        public GameScene Server { get { return server; } }

        public virtual void Load(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}