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
    [CreateAssetMenu(menuName = MenuPath + "Scores")]
	public class ScoresCore : Core.Module
	{
        [SerializeField]
        UDictionary<string, Entry> dictionary;
        public UDictionary<string, Entry> Dictioanry => dictionary;

        [Serializable]
        public struct Entry
        {
            [SerializeField]
            string name;
            public string Name => name;

            [SerializeField]
            int value;
            public int Value => value;

            public override string ToString() => $"{Name}: {Value}";

            public Entry(string name, int value)
            {
                this.name = name;
                this.value = value;
            }
        }

        public override void Init()
        {
            base.Init();

            Debug.Log(GetTop(3).ToCollectionString());
        }

        public void Submit(string name, int value)
        {
            var id = FormatID(name);

            dictionary[id] = new Entry(name, value);
        }

        public bool Remove(string name)
        {
            return dictionary.Remove(name);
        }

        public IList<Entry> GetTop(int count)
        {
            var target = dictionary
                .Values
                .OrderByDescending(x => x.Value)
                .Take(count)
                .ToArray();

            return target;
        }

        public static string FormatID(string name) => name.ToLower();
    }
}