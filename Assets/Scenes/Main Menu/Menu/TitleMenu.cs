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

            Visible = false;
            MainMenu.Start.Visible = true;
        }
    }
}