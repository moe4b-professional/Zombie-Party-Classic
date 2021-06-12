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
    [RequireComponent(typeof(AudioSource))]
	public class LevelMusic : MonoBehaviour
	{
		[SerializeField]
        AudioClip clip = default;
        public AudioClip Clip => clip;

        AudioSource AudioSource;

        public float Volume
        {
            get => AudioSource.volume;
            set => AudioSource.volume = value;
        }

        Level Level => Level.Instance;

        void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            Level.PlayStage.OnBegin += PlayCallback;
            Level.EndStage.OnBegin += EndCallback;

            Level.OnExit += ExitCallback;

            Level.Pause.OnStateChange += PauseStateCallback;
        }

        void PauseStateCallback(LevelPauseState state)
        {
            switch (state)
            {
                case LevelPauseState.None:
                    AudioSource.UnPause();
                    break;

                case LevelPauseState.Full:
                    AudioSource.Pause();
                    break;
            }
        }

        void PlayCallback()
        {
            AudioSource.clip = clip;
            AudioSource.loop = true;

            AudioSource.Play();
        }

        void EndCallback() => Fade(Volume / 5f);

        void ExitCallback() => Fade(0f);

        Coroutine Fade(float target)
        {
            StopAllCoroutines();

            return StartCoroutine(Procedure(target));
        }
        IEnumerator Procedure(float target)
        {
            var start = Volume;

            var duration = 2f;
            var timer = 0f;

            while (timer != duration)
            {
                timer = Mathf.MoveTowards(timer, duration, Time.unscaledDeltaTime);

                Volume = Mathf.Lerp(start, target, timer / duration);

                yield return new WaitForEndOfFrame();
            }
        }
    }
}