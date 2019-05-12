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
	public class LevelEndStage : LevelStage
	{
        public override LevelStage Next { get { return null; } }

        public override void Begin()
        {
            base.Begin();

            Level.Spawner.Stop();

            Menu.HUD.Visible = false;
            Menu.End.Visible = true;

            End();
        }
    }
}