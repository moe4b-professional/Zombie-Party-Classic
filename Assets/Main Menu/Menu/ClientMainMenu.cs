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
using UnityEngine.Networking;

namespace Game
{
	public class ClientMainMenu : Menu
    {
        [SerializeField]
        protected PlayerNameInputField playerName;
        public PlayerNameInputField PlayerName { get { return playerName; } }

        [SerializeField]
        protected Button join;
        public Button Join { get { return join; } }

        public Popup Popup { get { return MainMenu.Instance.Popup; } }

        public Core Core { get { return Core.Asset; } }

        public ScenesCore Scenes { get { return Core.Scenes; } }

        public NetworkCore Network { get { return Core.Server; } }

        void Awake()
        {
            join.onClick.AddListener(OnJoin);
        }

        void OnJoin()
        {
            Popup.Visible = true;

            Popup.Label.text = "Searching For a Server";

            Popup.Button.Text = "Cancel";
            Popup.Button.onClick.AddListener(OnCancel);

            Popup.Interactable = true;

            Network.Client.PlayerDataConfirmation.Event += OnPlayerDataConfirmation;

            Network.Client.Join();
        }

        void OnPlayerDataConfirmation(NetworkMessage msg)
        {
            Popup.Visible = false;

            Network.Client.PlayerDataConfirmation.Event -= OnPlayerDataConfirmation;
            Popup.Button.onClick.RemoveListener(OnCancel);

            Scenes.Load(Scenes.Level.Name);
        }

        void OnCancel()
        {
            Network.Client.PlayerDataConfirmation.Event -= OnPlayerDataConfirmation;
            Popup.Button.onClick.RemoveListener(OnCancel);

            Network.Client.Stop();

            Scenes.Load(Scenes.MainMenu.Name);
        }

        void OnDestroy()
        {
            Network.Client.PlayerDataConfirmation.Event -= OnPlayerDataConfirmation;
        }
    }
}