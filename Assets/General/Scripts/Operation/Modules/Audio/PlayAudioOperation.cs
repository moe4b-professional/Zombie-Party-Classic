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
	public class PlayAudioOperation : Operation.Behaviour
	{
        public AudioClip clip;

        public AudioSource audioSource;

        public PlaybackMode playMode = PlaybackMode.AdditiveOneShot;
        public enum PlaybackMode
        {
            Loop, AdditiveOneShot, OverridingOneShot
        }

        protected virtual void Reset()
        {
            audioSource = Dependancy.Get<AudioSource>(gameObject);
        }

        public override void Execute()
        {
            if (audioSource == null)
                throw Dependancy.FormatException(nameof(audioSource), this);

            if (clip == null)
                throw Dependancy.FormatException(nameof(clip), this);

            switch (playMode)
            {
                case PlaybackMode.Loop:
                    if (audioSource.isPlaying) audioSource.Stop();
                    audioSource.clip = clip;
                    audioSource.loop = true;
                    audioSource.Play();
                    break;

                case PlaybackMode.OverridingOneShot:
                case PlaybackMode.AdditiveOneShot:
                    if (playMode == PlaybackMode.OverridingOneShot && audioSource.isPlaying) audioSource.Stop();
                    audioSource.PlayOneShot(clip);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}