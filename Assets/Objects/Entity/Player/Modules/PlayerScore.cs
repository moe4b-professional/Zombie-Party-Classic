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
    public class PlayerScore : MonoBehaviour, IPlayerReference
    {
        [SerializeField]
        int value;
        public int Value => value;

        Player Player;
        public void Init(Player reference)
        {
            Player = reference;
        }

        public Core Core => Core.Asset;

        void Start()
        {
            Player.OnDeath += OnPlayerDied;
        }

        void OnPlayerDied(Entity damager) => Submit();
        void Submit() => Core.Scores.Submit(Player.Client.Name, value);

        public void Add(int increase)
        {
            value += increase;
        }
    }
}