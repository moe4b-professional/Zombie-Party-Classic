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
using UnityEngine.Networking.NetworkSystem;

namespace Game
{
	public class ServerMainMenu : Menu
	{
        public Core Core { get { return Core.Asset; } }
        public ScenesCore Scenes { get { return Core.Scenes; } }
        public NetworkCore Network { get { return Core.Server; } }

        public Popup Popup { get { return MainMenu.Instance.Popup; } }

        void OnEnable()
        {
            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            Popup.Visible = true;

            Popup.Label.text = "Starting Server";

            Popup.Interactable = false;

            yield return new WaitForSeconds(1f);

            Network.Server.Start();

            yield return null;

            Scenes.Load(Scenes.Level.Name);
        }
    }
}