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

using Newtonsoft.Json;

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
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public struct Entry
        {
            [JsonProperty]
            [SerializeField]
            string name;
            public string Name => name;

            [JsonProperty]
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

        public const string ID = "Top Scores";

        public override void Init()
        {
            base.Init();

            Load();
        }

        public void Save()
        {
            AutoPrefs.Set(ID, dictionary.Values);
        }

        public void Load()
        {
            if (AutoPrefs.Contains(ID) == false) return;

            var entires = AutoPrefs.Read<Entry[]>(ID);

            foreach (var data in entires)
            {
                var id = FormatID(data.Name);

                dictionary[id] = data;
            }
        }

        public void Submit(string name, int value)
        {
            var id = FormatID(name);

            if (Dictioanry.TryGetValue(id, out var entry))
                if (entry.Value >= value)
                    return;

            entry = new Entry(name, value);

            dictionary[id] = entry;

            Save();
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