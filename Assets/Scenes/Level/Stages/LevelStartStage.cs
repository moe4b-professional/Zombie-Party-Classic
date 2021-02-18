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
	public class LevelStartStage : LevelStage
	{
        public override LevelStage Next { get { return Level.PlayStage; } }

        public override void Begin()
        {
            base.Begin();

            Menu.Initial.Visible = true;
            Menu.HUD.Visible = false;

            Room.ReadyStateChangedEvent += OnClientReadyStateChanged;
            Room.DisconnectionEvent += OnClientDisconnection;
        }

        void OnClientReadyStateChanged(Client client)
        {
            CheckReadiness();
        }

        void OnClientDisconnection(Client client)
        {
            CheckReadiness();
        }

        void CheckReadiness()
        {
            if (Room.Occupancy > 0 && Room.Ready)
            {
                Room.ReadyStateChangedEvent -= OnClientReadyStateChanged;
                Room.DisconnectionEvent -= OnClientDisconnection;

                OnReady();
            }
        }

        protected virtual void OnReady()
        {
            Menu.Initial.Visible = false;
            Menu.HUD.Visible = true;

            SpawnAllClients();

            End();
        }

        void SpawnAllClients()
        {
            for (int i = 0; i < Room.Clients.Count; i++)
                Level.Observers.Spawn(Room.Clients[i]);
        }

        void OnDestroy()
        {
            Room.ReadyStateChangedEvent -= OnClientReadyStateChanged;
            Room.DisconnectionEvent -= OnClientDisconnection;
        }
    }
}