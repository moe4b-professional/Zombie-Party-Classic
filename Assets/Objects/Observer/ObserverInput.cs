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
	public class ObserverInput : NetworkBehaviour, Observer.IReference
	{
        public Vector2 Move { get; protected set; }
        [Command]
        protected virtual void CmdSetMove(Vector2 newValue)
        {
            this.Move = newValue;
        }

        public Vector2 Look { get; protected set; }
        [Command]
        protected virtual void CmdSetLook(Vector2 newValue)
        {
            this.Look = newValue;
        }

        public Core Core { get { return Core.Asset; } }
        public NetworkCore Network { get { return Core.Server; } }

        public LevelMenu Menu { get { return LevelMenu.Instance; } }
        public ClientHUDLevelMenu HUD { get { return Menu.Client.HUD; } }

        Observer observer;

        public virtual void Init(Observer observer)
        {
            this.observer = observer;
        }

        protected virtual void Update()
        {
            if(isLocalPlayer)
            {
                if (Network.Client.Active)
                {
                    if(Network.Client.Active)
                    {
                        CmdSetMove(HUD.Move.Value);

                        CmdSetLook(HUD.Look.Value);
                    }
                }
            }
        }
    }
}