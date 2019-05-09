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
    [DefaultExecutionOrder(Level.ExecutionOrder)]
    public class LevelSetup : MonoBehaviour
    {
        public LevelMenu GameMenu { get { return LevelMenu.Instance; } }
        public Popup Popup { get { return GameMenu.Popup; } }

        public Core Core { get { return Core.Asset; } }
        public ScenesCore Scenes { get { return Core.Scenes; } }
        public NetworkCore Network { get { return Core.Server; } }

        void Start()
        {
            if (Network.Server.Active)
            {
                AddScene(Scenes.Server.Name);

                Network.Server.Broadcast();
            }
            
            if(Network.Client.Active)
            {
                AddScene(Scenes.Client.Name);

                Network.Client.DisconnectedEvent.Event += OnClientDisconnected;
            }

            SceneManager.sceneLoaded += SetActiveScene;
        }

        Scene AddScene(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);

            return SceneManager.GetSceneByName(name);
        }
        void SetActiveScene(Scene scene, LoadSceneMode loadMode)
        {
            SceneManager.sceneLoaded -= SetActiveScene;

            SceneManager.SetActiveScene(scene);

            OnSceneLoaded();
        }

        void OnSceneLoaded()
        {
            GameMenu.Configure();
        }

        void OnClientDisconnected(NetworkMessage msg)
        {
            Network.Client.DisconnectedEvent.Event -= OnClientDisconnected;

            Popup.Visible = true;

            Popup.Label.text = "Disconnected";

            Popup.Interactable = true;

            Popup.Button.Text = "Ok";
            Popup.Button.onClick.AddListener(ReturnToMainMenu);
        }

        public virtual void ReturnToMainMenu()
        {
            Network.Stop();

            Scenes.Load(Scenes.MainMenu.Name);
        }

        void OnDestroy()
        {
            Network.Client.DisconnectedEvent.Event -= OnClientDisconnected;
        }
    }
}