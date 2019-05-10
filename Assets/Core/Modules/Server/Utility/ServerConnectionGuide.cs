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
	public class ServerConnectionGuide : MonoBehaviour
	{
        public Core Core { get { return Core.Asset; } }

        public int Port { get { return Core.WebServer.Port; } }
        public string Address { get { return Core.Server.Address; } }

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected RawImage image;
        public RawImage Image { get { return image; } }

        void Start()
        {
            label.text = Address + ":" + Port;

            image.texture = QRUtility.Generate("http://" + Address + ":" + Port, 256);
        }
	}
}