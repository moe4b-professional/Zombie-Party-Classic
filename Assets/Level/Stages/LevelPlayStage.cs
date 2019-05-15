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
	public class LevelPlayStage : LevelStage
	{
        public override LevelStage Next { get { return Level.EndStage; } }

        public PlayersManager Players { get { return Level.Players; } }

        public override void Begin()
        {
            base.Begin();

            Clients.Broadcast("#Start");

            Players.OnRemove += OnPlayerRemoved;
        }

        protected virtual void OnPlayerRemoved(Player player)
        {
            if (Players.List.Count == 0)
                End();
        }
    }
}