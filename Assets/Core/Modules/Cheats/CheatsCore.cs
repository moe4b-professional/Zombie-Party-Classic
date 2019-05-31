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
    [CreateAssetMenu(menuName = MenuPath + "Cheats")]
	public class CheatsCore : Core.Module
	{
        public static Level Level { get { return Level.Instance; } }

        [SerializeField]
        protected SoundsData sounds;
        public SoundsData Sounds { get { return sounds; } }
        [Serializable]
        public class SoundsData
        {
            [SerializeField]
            protected AudioClip on;
            public AudioClip On { get { return on; } }

            [SerializeField]
            protected AudioClip off;
            public AudioClip Off { get { return off; } }
        }
        protected virtual void PlayAudio(AudioClip clip)
        {
            SceneAccessor.AudioSource.Stop();

            SceneAccessor.AudioSource.PlayOneShot(clip);
        }

        public bool KeyDown
        {
            get
            {
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }

        public override void Configure()
        {
            base.Configure();

            SceneAccessor.UpdateEvent += Update;
        }

        public override void Init()
        {
            base.Init();

            AllPlayersArePyro = false;
        }

        protected virtual void Update()
        {
            UpdateVSync();

            UpdateAllPyro();

            UpdateHealth();

            UpdateSpawner();

            UpdateKillAll();
        }

        void UpdateVSync()
        {
            if (KeyDown && Input.GetKeyDown(KeyCode.V))
            {
                switch (QualitySettings.vSyncCount)
                {
                    case 0:
                        QualitySettings.vSyncCount = 1;
                        PlayAudio(sounds.On);
                        break;

                    case 1:
                        QualitySettings.vSyncCount = 0;
                        PlayAudio(sounds.Off);
                        break;

                    default:
                        QualitySettings.vSyncCount = 0;
                        PlayAudio(sounds.Off);
                        break;
                }
            }
        }

        public bool AllPlayersArePyro { get; private set; }
        void UpdateAllPyro()
        {
            if (Level == null) return;

            if (KeyDown && Input.GetKeyDown(KeyCode.P))
            {
                AllPlayersArePyro = !AllPlayersArePyro;

                if(AllPlayersArePyro) PlayAudio(sounds.On);
                else PlayAudio(sounds.Off);
            }
        }

        void UpdateHealth()
        {
            if (Level == null) return;

            if (KeyDown && Input.GetKeyDown(KeyCode.H))
            {
                if (Level.Players.List.Count == 0) return;

                foreach (var player in Level.Players.List)
                {
                    player.Health.MaxValue *= 2;
                    player.Health.Value = player.Health.MaxValue;
                }

                PlayAudio(sounds.On);
            }
        }

        void UpdateSpawner()
        {
            if (Level == null) return;

            if(KeyDown && Input.GetKeyDown(KeyCode.S))
            {
                KillAll<Zombie>();

                Level.Spawner.Stop();

                Level.Spawner.gameObject.SetActive(false);

                PlayAudio(sounds.On);
            }
        }

        void UpdateKillAll()
        {
            if (Level == null) return;

            if (KeyDown && Input.GetKeyDown(KeyCode.Z))
            {
                KillAll<Zombie>();

                PlayAudio(sounds.On);
            }
        }
        void KillAll<TTarget>()
            where TTarget : Entity
        {
            foreach (var target in FindObjectsOfType<TTarget>())
                target.Suicide();
        }
    }
}