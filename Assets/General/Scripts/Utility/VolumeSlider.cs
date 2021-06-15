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

using MB;

namespace Default
{
	[RequireComponent(typeof(Slider))]
	public class VolumeSlider : MonoBehaviour, IInitialize
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

		public void Configure()
        {
			slider = GetComponent<Slider>();
		}

		public void Init()
		{
			Load();

			slider.value = Volume;
			slider.onValueChanged.AddListener(ValueChange);
		}

		void Load()
        {
			Volume = PlayerPrefs.GetFloat(id, Volume);
		}

		void ValueChange(float value)
        {
			UpdateState();

			Save();
		}
		void UpdateState()
		{
			Volume = slider.value;
		}

		void Save()
		{
			PlayerPrefs.SetFloat(id, Volume);
		}
    }
}