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
	public class LevelPauseStateRelay : Relay
	{
        public LevelPauseState target = LevelPauseState.Full;

        protected override void Start()
        {
            base.Start();

            Level.Instance.Pause.OnStateChange += OnPauseChange;
        }

        void OnPauseChange(LevelPauseState state)
        {
            if(state == target)
                InvokeEvent();
        }
    }
}