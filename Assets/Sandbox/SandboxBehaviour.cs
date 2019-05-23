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
        public Weapon weapon;

        void Start()
        {
            weapon.Init(null);
        }

        void Update()
        {
            weapon.Process(Input.GetMouseButton(0));

            if (Input.GetKeyDown(KeyCode.D))
                SceneManager.LoadScene(Core.Asset.Scenes.Level.Name);
        }
    }
}