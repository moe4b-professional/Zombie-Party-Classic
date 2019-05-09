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
	public class KeyCodeInputRelay : Relay
	{
        public KeyCode key = KeyCode.Escape;

        public InputMode Mode = InputMode.Down;
        public enum InputMode
        {
            Down, Hold, Up
        }

        void Update()
        {
            switch (Mode)
            {
                case InputMode.Down:
                    {
                        if (Input.GetKeyDown(key))
                            InvokeEvent();
                    }
                    break;

                case InputMode.Hold:
                    {
                        if (Input.GetKey(key))
                            InvokeEvent();
                    }
                    break;

                case InputMode.Up:
                    {
                        if (Input.GetKeyUp(key))
                            InvokeEvent();
                    }
                    break;
            }
        }
	}
}