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
    public class EntityDeathScore : MonoBehaviour, IEntityReference
    {
        [SerializeField]
        int value = 1;

        Entity Entity;
        public void Init(Entity reference)
        {
            Entity = reference;
        }

        void Start()
        {
            Entity.OnDeath += DeathCallback;
        }

        void DeathCallback(Entity damager)
        {
            var player = damager as Player;

            if (player == null) return;

            player.Score.Add(value);
        }
    }
}