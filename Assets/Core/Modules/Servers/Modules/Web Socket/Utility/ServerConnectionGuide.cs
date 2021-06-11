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

namespace Default
{
	public class ServerConnectionGuide : MonoBehaviour, IPointerClickHandler
	{
        [SerializeField]
        protected UIData _UI;
        public UIData UI { get { return _UI; } }
        [Serializable]
        public class UIData
        {
            [SerializeField]
            protected Text address;
            public Text Address { get { return address; } }

            [SerializeField]
            protected Text _URL;
            public Text URL { get { return _URL; } }

            [SerializeField]
            protected RawImage _QR;
            public RawImage QR { get { return _QR; } }
        }

        public Core Core { get { return Core.Asset; } }

        public IPAddress Address { get { return Core.Servers.Address; } }

        public int Port { get { return Core.Servers.WebServer.Port; } }

        public string EndPoint
        {
            get
            {
                if (Port == 80)
                    return Address.ToString();
                else
                    return Address.ToString() + ":" + Port.ToString();
            }
        }

        public string URL { get { return "http://" + EndPoint; } }

        void Start()
        {
            UI.Address.text = EndPoint;

            UI.URL.text = Core.Servers.DNS.URL;

            UI.QR.texture = QRUtility.Generate(URL, 256);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(Debug.isDebugBuild || Application.isEditor || eventData.button == PointerEventData.InputButton.Left)
                Application.OpenURL(URL);
        }
    }
}