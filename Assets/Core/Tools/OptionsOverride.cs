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

using Newtonsoft.Json.Linq;

namespace Game
{
	public static class OptionsOverride
	{
        public const string FileName = "Options Override.json";

        public static string FilePath
        {
            get
            {
                return Path.Combine(Application.streamingAssetsPath, FileName);
            }
        }

        public static Dictionary<string, JToken> Entries { get; private set; }

        public static void Configure()
        {
            Entries = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);

            if (File.Exists(FilePath))
            {
                var text = File.ReadAllText(FilePath);

                var jObject = JObject.Parse(text);

                foreach (var pair in jObject)
                    Entries.Add(pair.Key, pair.Value);
            }
            else
            {
                Debug.LogWarning("Configuring Options Overrie but no file was found is Streaming Assets");
            }
        }

        public static bool Contains(string key)
        {
            return Entries.ContainsKey(key);
        }
        
        public static JToken Get(string key)
        {
            if (!Entries.ContainsKey(key))
                throw new KeyNotFoundException("No Override Found with Key: " + key);

            return Entries[key];
        }
        public static TType Get<TType>(string key)
        {
            var value = Get(key);

            return value.ToObject<TType>();
        }

        public static TType Get<TType>(string key, TType defaultValue)
        {
            if (Entries.ContainsKey(key))
                return Get<TType>(key);
            else
                return defaultValue;
        }
    }
}