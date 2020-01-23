#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

using UnityEditor;
using UnityEditorInternal;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;

namespace Game
{
    [CreateAssetMenu]
    public class CompilationMusic : ScriptableObject
    {
        public const string ScriptName = "Compilation Music";

        static CompilationMusic _asset;
        public static CompilationMusic Asset
        {
            get
            {
                if (_asset == null)
                {
                    var list = Resources.LoadAll<CompilationMusic>("");

                    if (list.Length > 0)
                        _asset = list.First();
                }

                return _asset;
            }
        }

        [SerializeField]
        protected bool enabled = true;

        [SerializeField]
        [Range(0f, 1f)]
        protected float volume = 1f;

        [SerializeField]
        protected DingData ding;
        public DingData Ding { get { return ding; } }
        [Serializable]
        public class DingData
        {
            [SerializeField]
            protected bool enabled = true;
            public bool Enabled { get { return enabled; } }

            [SerializeField]
            protected AudioClip clip;
            public AudioClip Clip { get { return clip; } }
        }

        [SerializeField]
        protected TracksData tracks;
        public TracksData Tracks { get { return tracks; } }
        [Serializable]
        public class TracksData
        {
            [SerializeField]
            protected AudioClip[] list;
            public AudioClip[] List { get { return list; } }

            [SerializeField]
            protected bool shuffle = true;
            public bool Shuffle { get { return shuffle; } }

            public AudioClip Get()
            {
                if (list.Length == 0) return null;
                if (list.Length == 1) return list.First();

                if (shuffle)
                    return list[Random.Range(0, list.Length)];
                else
                    return list.First();
            }
        }

        [MenuItem("Tools/" + ScriptName)]
        public static void OnMenuItem()
        {
            Selection.activeObject = Asset;
        }

        [InitializeOnLoadMethod]
        public static void Loaded()
        {
            CompilationPipeline.assemblyCompilationStarted += OnCompilationStart;
        }

        static void OnCompilationStart(string obj)
        {
            if (Asset.enabled == false) return;

            var scene = EditorSceneManager.GetSceneByName(string.Empty);
            AudioSource audioSource = null;

            if (scene.isLoaded)
                audioSource = FindAudioSource(scene);
            else
                scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            if (audioSource == null)
                audioSource = CreateAudioSource(scene);

            audioSource.volume = Asset.volume;

            if (audioSource.isPlaying == false)
            {
                audioSource.clip = Asset.tracks.Get();
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        [DidReloadScripts]
        static void OnScriptsReload()
        {
            var scene = EditorSceneManager.GetSceneByName(string.Empty);

            if (scene.isLoaded == false) return;

            var audioSource = FindAudioSource(scene);

            if (audioSource != null)
            {
                if (Asset.enabled && Asset.ding.Enabled)
                {
                    WaitFor(0.2f, PlaySFX);

                    void PlaySFX()
                    {
                        audioSource.Stop();

                        audioSource.PlayOneShot(Asset.ding.Clip);

                        var timer = new Timer(End);
                        timer.Start(Asset.ding.Clip.length);
                    }
                }
                else
                {
                    End();
                }
            }
        }

        static void End()
        {
            var scene = EditorSceneManager.GetSceneByName(string.Empty);

            if (scene.isLoaded == false) return;

            var audioSource = FindAudioSource(scene);

            if (audioSource != null)
            {
                if (scene.rootCount == 1 && EditorSceneManager.loadedSceneCount > 1)
                {
#pragma warning disable CS0618
                    EditorSceneManager.UnloadScene(scene);
#pragma warning restore CS0618
                }
                else
                    GameObject.DestroyImmediate(audioSource.gameObject);
            }
        }

        public static T GetResource<T>(string name)
            where T : Object
        {
            T resource = Resources.Load<T>(name);

            if (Object.Equals(resource, null))
                throw new FileNotFoundException("An asset of type: " + typeof(T).Name + " file named \"" + name + "\" needs to be located in a Resources folder");

            return resource;
        }

        public const string AudioSourceName = ScriptName + " Audio";
        public static AudioSource FindAudioSource(Scene scene)
        {
            foreach (var gameobject in scene.GetRootGameObjects())
                if (gameobject.name == AudioSourceName)
                    return gameobject.GetComponent<AudioSource>();

            return null;
        }
        public static AudioSource CreateAudioSource(Scene scene)
        {
            var audioSource = new GameObject(AudioSourceName).AddComponent<AudioSource>();

            EditorSceneManager.MoveGameObjectToScene(audioSource.gameObject, scene);

            return audioSource;
        }

        static void WaitFor(float time, Action action)
        {
            var timer = new Timer(action);
            timer.Start(time);
        }

        public class Timer
        {
            public float TimeSinceStartup { get { return (float)EditorApplication.timeSinceStartup; } }

            public Action callback;

            public float endTime;

            public void Start(float duration)
            {
                this.endTime = TimeSinceStartup + duration;

                EditorApplication.update += Update;
            }

            void Update()
            {
                if (TimeSinceStartup >= endTime)
                {
                    End();

                    callback();
                }
            }

            public void End()
            {
                EditorApplication.update -= Update;
            }

            public Timer(Action callback)
            {
                this.callback = callback;
            }
        }
    }
}
#endif