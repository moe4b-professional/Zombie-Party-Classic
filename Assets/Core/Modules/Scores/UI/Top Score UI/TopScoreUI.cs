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
	public class TopScoreUI : UIElement
	{
		[SerializeField]
		GameObject template;

        [SerializeField]
        RectTransform layout;

        [SerializeField]
        int count = 5;

        public List<TopScoreEntryUITemplate> Entries { get; protected set; }

        Core Core => Core.Asset;

        void OnEnable()
        {
            UpdateState();
        }

        void UpdateState()
        {
            Clear();

            var list = Core.Scores.GetTop(count);
            AddAll(list);
        }

        void AddAll(IList<ScoresCore.Entry> entries)
        {
            foreach (var item in entries)
                Create(item);
        }

        TopScoreEntryUITemplate Create(ScoresCore.Entry entry)
        {
            var instance = Instantiate(template);
            instance.transform.SetParent(layout, false);

            var script = instance.GetComponent<TopScoreEntryUITemplate>();
            script.Set(entry);

            Entries.Add(script);

            return script;
        }

        void Clear()
        {
            foreach (var item in Entries)
                Destroy(item.gameObject);

            Entries.Clear();
        }

        public TopScoreUI()
        {
            Entries = new List<TopScoreEntryUITemplate>();
        }
    }
}