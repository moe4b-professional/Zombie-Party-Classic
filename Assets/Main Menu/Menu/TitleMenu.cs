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

using UnityEngine.EventSystems;

namespace Game
{
    public class TitleMenu : Menu, IPointerClickHandler
    {
        public MainMenu MainMenu { get { return MainMenu.Instance; } }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            if (Application.isMobilePlatform || !Application.isEditor)
                ProgressToClient();
            else
                ProgressToServer();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                ProgressToClient();
            else if (Input.GetKeyDown(KeyCode.S))
                ProgressToServer();
        }

        void ProgressToClient()
        {
            MainMenu.Client.Visible = true;

            Visible = false;
        }

        void ProgressToServer()
        {
            MainMenu.Server.Visible = true;

            Visible = false;
        }
    }
}