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

        Level Level => Level.Instance;

        void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            Level.PlayStage.OnBegin += PlayCallback;
            Level.EndStage.OnBegin += EndCallback;
        }

        void PlayCallback()
        {
            AudioSource.clip = clip;
            AudioSource.loop = true;

            AudioSource.Play();
        }

        void EndCallback()
        {
            StartCoroutine(Procedure());
            IEnumerator Procedure()
            {
                var start = AudioSource.volume;
                var target = 0f;

                var duration = 1f;
                var timer = 0f;

                while(timer != duration)
                {
                    timer = Mathf.MoveTowards(timer, duration, Time.unscaledDeltaTime);

                    AudioSource.volume = Mathf.Lerp(start, target, timer / duration);

                    yield return new WaitForEndOfFrame();
                }

                AudioSource.Stop();
                AudioSource.volume = start;
            }
        }
    }
}