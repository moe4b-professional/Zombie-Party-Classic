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
	public class RandomGameObject : MonoBehaviour
	{
		[SerializeField]
        protected GameObject[] list;
        public GameObject[] List { get { return list; } }

        protected virtual void Awake()
        {
            var index = Random.Range(0, list.Length);

            for (int i = 0; i < list.Length; i++)
                list[i].SetActive(i == index);
        }
    }
}