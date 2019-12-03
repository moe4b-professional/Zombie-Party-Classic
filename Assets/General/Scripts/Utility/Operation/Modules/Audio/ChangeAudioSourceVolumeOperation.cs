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
	public class ChangeAudioSourceVolumeOperation : Operation.Behaviour
	{
        public AudioSource audioSource;

        [SerializeField]
        protected float transitionDuration = 0f;
        public float TransitionDuration
        {
            get
            {
                return transitionDuration;
            }
            set
            {
                if (value < 0f)
                    value = 0f;

                transitionDuration = value;
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        protected float targetVolume = 0.5f;
        public float TargetVolume
        {
            get
            {
                return targetVolume;
            }
            set
            {
                value = Mathf.Clamp01(value);

                targetVolume = value;
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
            var initialVolume = audioSource.volume;

            if (transitionDuration > 0f)
            {
                var timer = 0f;

                while (timer != transitionDuration)
                {
                    timer = Mathf.MoveTowards(timer, transitionDuration, Time.deltaTime);

                    audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, timer / transitionDuration);

                    yield return new WaitForEndOfFrame();
                }
            }

            audioSource.volume = targetVolume;
        }
    }
}