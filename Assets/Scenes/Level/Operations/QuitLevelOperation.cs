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
	public class QuitLevelOperation : Operation.Behaviour
	{
        public Level Level { get { return Level.Instance; } }

        public LevelMenu Menu { get { return Level.Menu; } }

        public Core Core { get { return Core.Asset; } }
        public ScenesCore Scenes { get { return Core.Scenes; } }

        public override void Execute() => Level.Quit();
    }
}