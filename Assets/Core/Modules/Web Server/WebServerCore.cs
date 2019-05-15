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

using System.Net;

using NHttp;

namespace Game
{
    [CreateAssetMenu(menuName = MenuPath + "Web Server")]
	public class WebServerCore : Core.Module
	{
        [SerializeField]
        protected int port = 7878;
        public int Port { get { return port; } }

        HttpServer server;

        public string Root { get; protected set; }

        public bool Active
        {
            get
            {
                if (server == null) return false;

                return server.State == HttpServerState.Started;
            }
        }

        public override void Configure()
        {
            base.Configure();

            Root = Application.streamingAssetsPath;

            Mimes.Configure();

            Core.SceneAccessor.ApplicationQuitEvent += OnApplicationQuit;
        }

        public void Start()
        {
            server = new HttpServer();

            server.EndPoint = new IPEndPoint(IPAddress.Any, port);
            server.RequestReceived += OnRequest;

            server.Start();
        }

        private void OnRequest(object sender, HttpRequestEventArgs args)
        {
            string resourcePath = Root + "/Web-Server" + args.Request.Path;

            if (args.Request.Path == "/")
                resourcePath += "index.html";

            string extension = Path.GetExtension(resourcePath);

            if (File.Exists(resourcePath))
            {
                args.Response.StatusCode = 200;
                args.Response.StatusDescription = "OK.";

                string mime;

                try
                {
                    mime = Mimes.FindByExtension(extension).Value;
                }
                catch (Exception)
                {
                    mime = null;
                }

                if(mime != null)
                    args.Response.ContentType = mime;

                using (FileStream file = File.OpenRead(resourcePath))
                {
                    var length = (int)file.Length;
                    
                    byte[] buffer;

                    using (BinaryReader reader = new BinaryReader(file))
                    {
                        buffer = reader.ReadBytes(length);
                    }

                    args.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                args.Response.StatusCode = 404;
                args.Response.StatusDescription = "Not Found";
            }
        }

        public void Stop()
        {
            if (!Active) return;

            server.Dispose();
        }
        void OnApplicationQuit()
        {
            Stop();
        }
    }
}