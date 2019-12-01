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
using System.Text;
using System.Text.RegularExpressions;

using NHttp;

namespace Game
{
    [CreateAssetMenu(menuName = MenuPath + "Web Server")]
	public class WebServerCore : ServersCore.Module
	{
        [SerializeField]
        protected int port = 80;
        public int Port { get { return port; } }

        HttpServer server;

        public string Root => Application.streamingAssetsPath;

        public override bool Active
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

            port = OptionsOverride.Get("Web Server Port", port);

            Mimes.Configure();
        }

        public override void Start()
        {
            try
            {
                server = new HttpServer
                {
                    EndPoint = new IPEndPoint(IPAddress.Any, port)
                };

                server.RequestReceived += OnRequest;

                server.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Error when initiating web server, message: " + e.ToString());
                throw;
            }
        }

        void OnRequest(object sender, HttpRequestEventArgs args)
        {
            switch (args.Request.HttpMethod)
            {
                case "GET":
                    OnGetRequest(sender, args);
                    break;

                case "RSC":
                    OnResourceRequest(sender, args);
                    break;
            }
        }

        void OnGetRequest(object sender, HttpRequestEventArgs args)
        {
            string resourcePath = Root + "/Web-Server" + RestoreUrlSpaces(args.Request.Path);

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

                if (mime != null)
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
        void OnResourceRequest(object sender, HttpRequestEventArgs args)
        {
            var request = args.Request.Path.Substring(1);
            request = RestoreUrlSpaces(request);
            request = request.ToLower();

            if(request == "server port")
            {
                args.Response.StatusCode = 200;
                args.Response.StatusDescription = "OK.";

                var data = Encoding.UTF8.GetBytes(Core.Servers.WebSocket.Port.ToString());
                args.Response.OutputStream.Write(data, 0, data.Length);
            }
        }

        string RestoreUrlSpaces(string target) => target.Replace("%20", " ");

        public override void Stop()
        {
            if (!Active) return;

            server.Stop();

            server = null;
        }
    }
}