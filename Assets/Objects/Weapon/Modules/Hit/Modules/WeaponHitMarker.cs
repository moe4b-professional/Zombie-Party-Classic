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
using System.Threading.Tasks;

namespace Game
{
	public class WeaponHitMarker : Weapon.Module
	{
        [SerializeField]
        float interval = 0.1f;

        WeaponHitMarkerState state;

        public Player Player { get; protected set; }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ActionEvent += ActionCallback;
            weapon.Hit.OnInvoke += HitCallback;

            Player = weapon.Owner as Player;

            Poll();
        }

        void ActionCallback()
        {
            if (state == WeaponHitMarkerState.None) state = WeaponHitMarkerState.Action;
        }

        void HitCallback(WeaponHit.Data data)
        {
            if (data.Entity == null) return;

            state = WeaponHitMarkerState.Hit;
        }

        async void Poll()
        {
            while (true)
            {
                var delay = Mathf.RoundToInt(interval * 1000);
                if (delay == 0) delay = 1;

                await Task.Delay(delay);

                switch (state)
                {
                    case WeaponHitMarkerState.Action:
                        Send(false, 20, 10, 20);
                        break;

                    case WeaponHitMarkerState.Hit:
                        Send(true, 80, 20, 80);
                        break;
                }

                state = WeaponHitMarkerState.None;
            }
        }

        void Send(bool hit, params float[] pattern)
        {
            var message = new HitMarkerMessage(hit, pattern);
            Player.Client.Send(message);
        }
    }

    enum WeaponHitMarkerState
    { 
        None, Action, Hit
    }
}