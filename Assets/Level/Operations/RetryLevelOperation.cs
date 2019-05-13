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
	public class RetryLevelOperation : Operation.Behaviour
	{
        public Level Level { get { return Level.Instance; } }

        public Core Core { get { return Core.Asset; } }

        public ScenesCore Scenes { get { return Core.Scenes; } }

        public ServerCore Server { get { return Core.Server; } }
        public ClientsManagerCore Clients { get { return Server.Clients; } }

        public override void Execute()
        {
            Clients.SetAllClientsReadiness(false);

            Scenes.Load(Scenes.Level.Name);
        }
    }
}