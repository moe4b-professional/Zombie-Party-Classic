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

        void OnEnable()
        {
            All.Add(this);
        }

        public event Action OnProcess;
        protected virtual void Update()
        {
            if (OnProcess != null) OnProcess();
        }

        protected override void Death(Entity Damager)
        {
            base.Death(Damager);

            All.Remove(this);
        }

        void OnDisable()
        {
            All.Remove(this);
        }

        //Static Utility

        public static List<AI> All { get; protected set; }

        static AI()
        {
            All = new List<AI>();
        }

        public static class Query
        {
            public struct Data
            {
                public AI AI { get; private set; }

                public float Angle { get; private set; }
                public float Distance { get; private set; }

                public float Scale => (Angle * 4) + (Distance * 1);

                public Vector3 Direction { get; private set; }

                public Data(AI Target, float angle, float distance, Vector3 direction)
                {
                    this.AI = Target;
                    this.Angle = angle;
                    this.Distance = distance;
                    this.Direction = direction;
                }
            }

            public static Data Find(Vector3 position, Vector3 forward, float maxDistance, float maxAngle)
            {
                if (All.Count == 0) return default;

                var list = new List<Data>(All.Count);

                for (int i = 0; i < All.Count; i++)
                {
                    var distance = Vector3.Distance(position, All[i].transform.position);
                    if (distance > maxDistance) continue;

                    var direction = (All[i].transform.position - position).normalized;

                    var angle = Vector3.Angle(forward, direction);
                    if (angle > maxAngle) continue;

                    var data = new Data(All[i], angle, distance, direction);

                    list.Add(data);
                }

                list.Sort(Sort);
                int Sort(Data a, Data b) => a.Scale.CompareTo(b.Scale);

                return list.FirstOrDefault();
            }
        }
	}
}