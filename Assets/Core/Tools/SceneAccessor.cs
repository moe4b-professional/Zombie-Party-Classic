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
	public class SceneAccessor : MonoBehaviour
	{
        public static SceneAccessor Create()
        {
            var gameObject = new GameObject("Scene Accessor");

            var component = gameObject.AddComponent<SceneAccessor>();

            component.Configure();

            return component;
        }

        public AudioSource AudioSource { get; protected set; }

        protected virtual void Configure()
        {
            DontDestroyOnLoad(gameObject);

            AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.loop = false;
        }
    }
}