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
	public static class Initializer
	{
        public static void Configure()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            Init(scene);
        }

        public static void Init(Scene scene)
        {
            var roots = scene.GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
                Init(roots[i], true);
        }

        public static void Init(GameObject gameObject)
        {
            Init(gameObject, false);
        }
        public static void Init(GameObject gameObject, bool recurseToChildren)
        {
            Init(gameObject.GetComponents<Interface>());

            if (recurseToChildren)
                for (int i = 0; i < gameObject.transform.childCount; i++)
                    Init(gameObject.transform.GetChild(i).gameObject, true);
        }

        public static void Init(IList<Interface> targets)
        {
            for (int i = 0; i < targets.Count; i++)
                Init(targets[i]);
        }

        public static void Init(Interface target)
        {
            target.Init();
        }

        public interface Interface
        {
            void Init();
        }
	}
}