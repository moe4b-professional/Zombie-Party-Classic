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
	public class LevelPause : MonoBehaviour
	{
        public Level Level { get { return Level.Instance; } }

        [SerializeField]
        protected LevelPauseState _state;
        public LevelPauseState State
        {
            get
            {
                return _state;
            }
            set
            {
                SetState(value);
            }
        }

        public bool IsNone { get { return State == LevelPauseState.None; } }
        public bool IsSoft { get { return State == LevelPauseState.Soft; } }
        public bool IsFull { get { return State == LevelPauseState.Full; } }

        public delegate void StateChangeDelegate(LevelPauseState state);
        public event StateChangeDelegate OnStateChange;

        public virtual void SetState(LevelPauseState value)
        {
            _state = value;

            Time.timeScale = State == LevelPauseState.Full ? 0f : 1f;

            if (OnStateChange != null) OnStateChange(_state);
        }

        public virtual void Init()
        {

        }

        protected virtual void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }

    public enum LevelPauseState
    {
        None, Soft, Full
    }
}