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
	public class LevelMenu : MonoBehaviour
	{
        public const int ExecutionOrder = Level.ExecutionOrder + 10;

        [SerializeField]
        protected Menu players;
        public Menu Players { get { return players; } }

        [SerializeField]
        protected Menu _HUD;
        public Menu HUD { get { return _HUD; } }

        [SerializeField]
        protected PopupLabel waveLabel;
        public PopupLabel WaveLabel { get { return waveLabel; } }

        [SerializeField]
        protected Popup popup;
        public Popup Popup { get { return popup; } }

        public Core Core { get { return Core.Asset; } }

        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }
    }
}