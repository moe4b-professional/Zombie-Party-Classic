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

using System.Net;

namespace Game
{
	public class ServerConnectionGuide : MonoBehaviour, IPointerClickHandler
	{
        public Core Core { get { return Core.Asset; } }

        public IPAddress Address { get; protected set; }
        protected virtual void InitAddress()
        {
            var local = LocalAddress.Get();

            try
            {
                Address = OptionsOverride.Get("Display IP Address", IPAddress.Parse, local);
            }
            catch (Exception)
            {
                Debug.LogError("Error when getting Display IP Address, Using Local Address instead");

                Address = local;
            }
        }

        public int Port { get { return Core.WebServer.Port; } }

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
            InitAddress();

            label.text = EndPoint;

            image.texture = QRUtility.Generate(URL, 256);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(Debug.isDebugBuild || Application.isEditor || eventData.button == PointerEventData.InputButton.Left)
                Application.OpenURL(URL);
        }
    }
}