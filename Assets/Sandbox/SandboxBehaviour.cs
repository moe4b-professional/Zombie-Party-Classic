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
    [DefaultExecutionOrder(-200)]
	public class SandboxBehaviour : MonoBehaviour
	{
        public Weapon[] weapons;

        void Start()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].Init(null);
            }
        }

        void Update()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].Process(Input.GetMouseButton(0));
            }

            if (Input.GetKeyDown(KeyCode.D))
                SceneManager.LoadScene(Core.Asset.Scenes.Level.Name);
        }
    }
}