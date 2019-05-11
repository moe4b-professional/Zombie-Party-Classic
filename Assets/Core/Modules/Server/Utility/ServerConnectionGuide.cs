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

using UnityEngine.EventSystems;

namespace Game
{
	public class ServerConnectionGuide : MonoBehaviour, IPointerClickHandler
	{
        public Core Core { get { return Core.Asset; } }

        public int Port { get { return Core.WebServer.Port; } }
        public string Address { get { return Core.Server.Address; } }

        public string EndPoint { get { return Address + ":" + Port.ToString(); } }

        public string URL { get { return "http://" + EndPoint; } }

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        [SerializeField]
        protected RawImage image;
        public RawImage Image { get { return image; } }

        void Start()
        {
            label.text = EndPoint;

            image.texture = QRUtility.Generate(URL, 256);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(Debug.isDebugBuild || Application.isEditor || eventData.button == PointerEventData.InputButton.Left)
            {
                Application.OpenURL(URL);
            }
        }
    }
}