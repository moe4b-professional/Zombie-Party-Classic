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
	public class ClientHUDLevelMenu : Menu
	{
		[SerializeField]
        protected VirtualJoystick move;
        public VirtualJoystick Move { get { return move; } }

        [SerializeField]
        protected VirtualJoystick look;
        public VirtualJoystick Look { get { return look; } }

        [SerializeField]
        protected ProgressBar healthBar;
        public ProgressBar HealthBar { get { return healthBar; } }
    }
}