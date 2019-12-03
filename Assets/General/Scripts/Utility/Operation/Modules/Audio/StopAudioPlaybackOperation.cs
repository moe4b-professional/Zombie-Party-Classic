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
	public class StopAudioPlaybackOperation : Operation.Behaviour
	{
        public AudioSource audioSource;

        [SerializeField]
        protected float fadeOutDuration = 0f;
        public float FadeOutDuration
        {
            get
            {
                return fadeOutDuration;
            }
            set
            {
                if (value < 0f)
                    value = 0f;

                fadeOutDuration = value;
            }
        }

        protected virtual void Reset()
        {
            audioSource = Dependancy.Get<AudioSource>(gameObject);
        }

        public override void Execute()
        {
            if (audioSource == null)
                throw Dependancy.FormatException(nameof(audioSource), this);

            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            if (!audioSource.isPlaying)
                yield break;

            var initialVolume = audioSource.volume;

            if (fadeOutDuration > 0f)
            {
                var timer = fadeOutDuration;

                while(timer > 0f)
                {
                    timer = Mathf.MoveTowards(timer, 0f, Time.deltaTime);

                    audioSource.volume = Mathf.Lerp(0f, initialVolume, timer / fadeOutDuration);

                    yield return new WaitForEndOfFrame();
                }
            }

            audioSource.Stop();
            audioSource.volume = initialVolume;
        }
    }
}