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
	public class PlayersListElement : MonoBehaviour
	{
        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected Image image;
        public Image Image { get { return image; } }

        public Color Color
        {
            get
            {
                return image.color;
            }
            set
            {
                image.color = value;
            }
        }

        public Client Client { get; protected set; }

        public void Set(Client client)
        {
            this.Client = client;

            label.text = Client.Name;
        }
    }
}