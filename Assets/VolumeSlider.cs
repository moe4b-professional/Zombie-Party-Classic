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
	[RequireComponent(typeof(Slider))]
	public class VolumeSlider : MonoBehaviour
	{
		[SerializeField]
		AudioSource audioSource;

		float Volume
        {
			get => audioSource.volume;
			set => audioSource.volume = value;
        }

		[SerializeField]
		string id = "Audio Volume";

		Slider slider;

        void Awake()
        {
			slider = GetComponent<Slider>();
        }

        void Start()
        {
			Load();

			slider.value = Volume;

			slider.onValueChanged.AddListener(ValueChange);
        }

		void Load()
        {
			Volume = PlayerPrefs.GetFloat(id, Volume);
        }

		void Save()
        {
			PlayerPrefs.SetFloat(id, Volume);
        }

		void UpdateState()
        {
			audioSource.volume = slider.value;
		}

		void ValueChange(float value)
        {
			UpdateState();

			Save();
		}
    }
}