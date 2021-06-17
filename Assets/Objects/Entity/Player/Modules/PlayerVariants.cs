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
	public class PlayerVariants : MonoBehaviour
	{
        [SerializeField]
        protected Data[] list;
        public Data[] List { get { return list; } }
        [Serializable]
        public class Data
        {
            [SerializeField]
            protected GameObject model;
            public GameObject Model { get { return model; } }

            [SerializeField]
            protected Weapon weapon;
            public Weapon Weapon { get { return weapon; } }
        }

        public Data Selection => list[Player.Client.ID];

        Player Player;
		public virtual void Init(Player player)
        {
            this.Player = player;

            if (Core.Asset.Cheats.AllPlayersArePyro)
                Apply(3);
            else
                Apply(player.Client.ID);
        }

        protected virtual void Apply(int ID)
        {
            Apply(list[ID]);
        }
        protected virtual void Apply(Data data)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if(list[i] != data)
                {
                    list[i].Model.SetActive(false);
                    list[i].Weapon.gameObject.SetActive(false);
                }
            }

            data.Model.SetActive(true);
            Player.Burn.SetModel(data.Model);

            data.Weapon.gameObject.SetActive(true);
            Player.Weapons.Set(data.Weapon);
        }
	}
}