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

using DNS;
using DNS.Server;
using DNS.Client;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;

using System.Net;

using System.Threading;

namespace Game
{
    [CreateAssetMenu(menuName = MenuPath + "DNS")]
    public class DNSCore : ServersCore.Module, IQuestionAnswerer
    {
        [SerializeField]
        protected int port = 53;
        public int Port { get { return port; } }

        [SerializeField]
        protected string _URL = "game.com";
        public string URL { get { return _URL; } set { _URL = value; } }

        DnsServer server;

        public override bool Active
        {
            get
            {
                if (server == null) return false;

                return server.Run;
            }
        }

        public override void Configure()
        {
            base.Configure();

            port = OptionsOverride.Get("DNS Port", port);

            URL = OptionsOverride.Get("DNS URL", URL);
        }

        public override void Start()
        {
            thread = new Thread(ThreadProcedure);
            thread.Start();
        }

        Thread thread;
        protected virtual void ThreadProcedure()
        {
            server = new DnsServer(this, Address, port);

            server.Listen(port);
        }

        public IList<IResourceRecord> Get(Question question)
        {
            Debug.Log(question.Name.ToString());

            List<IResourceRecord> entries = new List<IResourceRecord>()
            {
                new IPAddressResourceRecord(question.Name, Address)
            };

            return entries;
        }

        public override void Stop()
        {
            if (Active == false) return;

            server.Close();
        }
    }
}