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
    [DefaultExecutionOrder(-400)]
	public class LoadMainMenuIfNetworkOff : MonoBehaviour
	{
        public Core Core { get { return Core.Asset; } }

        public ScenesCore Scenes { get { return Core.Scenes; } }

        public NetworkCore Network { get { return Core.Server; } }

		protected virtual void Awake()
        {
            if (!Core.Server.Active)
                Scenes.Load(Scenes.MainMenu.Name);
        }
	}
}