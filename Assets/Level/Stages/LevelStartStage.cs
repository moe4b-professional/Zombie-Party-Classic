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

            Clients.ReadyStateChangedEvent += OnClientReadyStateChanged;
            Clients.DisconnectionEvent += OnClientDisconnection;
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
            if (Clients.Count > 0 && Clients.AllReady)
            {
                Clients.ReadyStateChangedEvent -= OnClientReadyStateChanged;
                Clients.DisconnectionEvent -= OnClientDisconnection;

                OnReady();
            }
        }

        protected virtual void OnReady()
        {
            Menu.Initial.Visible = false;
            Menu.HUD.Visible = true;

            Level.Spawner.Begin();

            SpawnAllClients();

            End();
        }

        void SpawnAllClients()
        {
            for (int i = 0; i < Clients.List.Count; i++)
                Level.Observers.Spawn(Clients.List[i]);
        }

        void OnDestroy()
        {
            Clients.ReadyStateChangedEvent -= OnClientReadyStateChanged;
            Clients.DisconnectionEvent -= OnClientDisconnection;
        }
    }
}