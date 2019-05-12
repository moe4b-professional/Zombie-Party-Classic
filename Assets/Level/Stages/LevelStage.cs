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
	public abstract class LevelStage : MonoBehaviour
	{
		public Level Level { get { return Level.Instance; } }
        public LevelMenu Menu { get { return Level.Menu; } }

        public Core Core { get { return Core.Asset; } }
        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }

        public abstract LevelStage Next { get; }

        public virtual void Init()
        {

        }

        public virtual void Begin()
        {

        }

        public event Action OnEnd;
        public virtual void End()
        {
            if (Next != null) Next.Begin();

            if (OnEnd != null) OnEnd();
        }
	}
}