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
    [DefaultExecutionOrder(ExecutionOrder)]
	public class LevelMenu : MonoBehaviour
	{
        public const int ExecutionOrder = Level.ExecutionOrder + 10;

        public static LevelMenu Instance { get; protected set; }

        public ClientLevelMenu Client { get; protected set; }

        public ServerLevelMenu Server { get; protected set; }

        [SerializeField]
        protected PopupLabel waveLabel;
        public PopupLabel WaveLabel { get { return waveLabel; } }

        [SerializeField]
        protected Popup popup;
        public Popup Popup { get { return popup; } }

        public Core Core { get { return Core.Asset; } }

        public NetworkCore Network { get { return Core.Server; } }

        void Awake()
        {
            Instance = this;
        }

        public virtual void Configure()
        {
            Client = FindObjectOfType<ClientLevelMenu>();

            Server = FindObjectOfType<ServerLevelMenu>();
        }
    }
}