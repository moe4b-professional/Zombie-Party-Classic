﻿using System;
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
	public class ZombieBody : MonoBehaviour
	{
		[SerializeField]
        protected GameObject[] models;
        public GameObject[] Models { get { return models; } }

        public GameObject Model { get; protected set; }

        [SerializeField]
        protected EntityBurn burn;
        public EntityBurn Burn { get { return burn; } }

        protected virtual void Awake()
        {
            var index = Random.Range(0, models.Length);

            for (int i = 0; i < models.Length; i++)
                if (i != index)
                    models[i].SetActive(false);

            Model = models[index];
            Model.SetActive(true);

            burn.SetModel(Model);
        }
    }
}