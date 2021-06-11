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
	public class AI : Entity
	{
        public Level Level { get { return Level.Instance; } }

        public PlayersManager Players { get { return Level.Players; } }

        public Core Core { get { return Core.Asset; } }

        protected override void Start()
        {
            base.Start();

            References.Init(this);
        }

        public event Action OnProcess;
        protected virtual void Update()
        {
            if (OnProcess != null) OnProcess();
        }
	}
}