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
	public class LevelEndMenu : Menu
	{
        public Level Level { get { return Level.Instance; } }

        [SerializeField]
        protected Text wave;
        public Text Wave { get { return wave; } }

        public override void Show()
        {
            base.Show();

            wave.text = PopupLabel.Colorize((Level.Spawner.WaveNumber - 1).ToString(), "red") + " Waves Survived";
        }
    }
}