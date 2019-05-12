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
        [SerializeField]
        protected Menu initial;
        public Menu Initial { get { return initial; } }

        [SerializeField]
        protected Menu _HUD;
        public Menu HUD { get { return _HUD; } }

        [SerializeField]
        protected LevelEndMenu end;
        public LevelEndMenu End { get { return end; } }

        [SerializeField]
        protected PopupLabel popupLabel;

        public PopupLabel PopupLabel { get { return popupLabel; } }

        [SerializeField]
        protected Popup popup;
        public Popup Popup { get { return popup; } }

        public Core Core { get { return Core.Asset; } }

        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }

        public virtual void Init()
        {

        }
    }
}