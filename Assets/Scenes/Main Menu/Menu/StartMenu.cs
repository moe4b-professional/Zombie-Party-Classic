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
    public class StartMenu : Menu
    {
        public MainMenu MainMenu { get { return MainMenu.Instance; } }

        public Core Core { get { return Core.Asset; } }
        public Popup Popup => Core.UI.Popup;

        public ServersCore Servers { get { return Core.Servers; } }
        public ScenesCore Scenes { get { return Core.Scenes; } }

        void OnEnable()
        {
            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            Popup.Show("Starting Server");

            yield return new WaitForSeconds(1f);

            try
            {
                Servers.Start();
            }
            catch (Exception)
            {
                Popup.Show(PopupLabel.Colorize("Server Error", "red"), OnError, "Close");
                throw;
            }

            Popup.Visible = false;

            Scenes.Load(Scenes.Level);
        }

        void OnError()
        {
            Popup.Hide();

            Visible = false;
            MainMenu.Title.Visible = true;
        }
    }
}