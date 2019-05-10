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
    public class StartMenu : Menu
    {
        public MainMenu MainMenu { get { return MainMenu.Instance; } }

        public Popup Popup { get { return MainMenu.Popup; } }

        public Core Core { get { return Core.Asset; } }

        public ServerCore Server { get { return Core.Server; } }
        public ScenesCore Scenes { get { return Core.Scenes; } }

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

            Server.Start();

            yield return null;

            Scenes.Load(Scenes.Level.Name);
        }
    }
}