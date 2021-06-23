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
        TopScoreUI topScore;

        public Core Core { get { return Core.Asset; } }

        public WebSocketServerCore WebSocketServer { get { return Core.Servers.WebSocket; } }
        public RoomCore Room { get { return Core.Room; } }

        public virtual void Init()
        {
            
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                topScore.Toggle();
        }
    }
}