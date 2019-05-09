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
    public class ClientLevelMenu : Menu
    {
        [SerializeField]
        protected Menu ready;
        public Menu Ready { get { return ready; } }

        [SerializeField]
        protected ClientHUDLevelMenu _HUD;
        public ClientHUDLevelMenu HUD { get { return _HUD; } }

        public LevelMenu GameMenu { get { return LevelMenu.Instance; } }
        public Popup Popup { get { return GameMenu.Popup; } }

        public Core Core { get { return Core.Asset; } }
        public NetworkCore Network { get { return Core.Server; } }


        void OnEnable()
        {
            ready.Visible = true;
        }
	}
}