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

using MB;

namespace Default
{
	public class ShowAndHide : MonoBehaviour
	{
        [SerializeField]
        bool active = true;
        public bool Active
        {
            get => active;
            set
            {
                active = value;

                UpdateState();
            }
        }

        [SerializeField]
        List<Renderer> renderers = new List<Renderer>();
        public List<Renderer> Renderers => renderers;

        [SerializeField]
        List<Graphic> graphics = new List<Graphic>();
        public List<Graphic> Graphics => graphics;

        void OnValidate()
        {
            var targets = QueryComponents.In<Component>(this, QueryComponents.Self | ComponentQueryScope.Children);

            renderers.Clear();
            graphics.Clear();

            foreach (var item in targets)
            {
                if (item is Renderer renderer)
                    renderers.Add(renderer);

                if (item is Graphic graphic)
                    graphics.Add(graphic);
            }

            UpdateState();
        }

        public void UpdateState()
        {
            for (int i = 0; i < renderers.Count; i++)
                renderers[i].enabled = active;

            for (int i = 0; i < graphics.Count; i++)
                graphics[i].enabled = active;
        }
    }
}