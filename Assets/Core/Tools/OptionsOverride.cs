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
	public static class OptionsOverride
	{
        public const string FileName = "Options Override.txt";

        public static string FilePath
        {
            get
            {
                return Path.Combine(Application.streamingAssetsPath, FileName);
            }
        }

        public static Dictionary<string, string> Entries { get; private set; }

        public static void Configure()
        {
            if (File.Exists(FilePath))
            {
                Entries = Header.ParseAll(File.ReadAllText(FilePath));
            }
            else
            {
                Entries = null;

                Debug.LogWarning("Configuring Options Overrie but no file was found is Streaming Assets");
            }
        }

        public static bool Contains(string key)
        {
            return Entries.ContainsKey(key);
        }
        
        public static string Get(string key)
        {
            if (!Entries.ContainsKey(key))
                throw new KeyNotFoundException("No Override Found with Key: " + key);

            return Entries[key];
        }
        public static T Get<T>(string key, Func<string, T> parser)
        {
            var value = Get(key);

            return parser(value);
        }

        public static string Get(string key, string defaultValue)
        {
            if (Entries.ContainsKey(key))
                return Get(key);
            else
                return defaultValue;
        }
        public static T Get<T>(string key, Func<string, T> parser, T defaultValue)
        {
            if (Entries.ContainsKey(key))
                return Get(key, parser);
            else
                return defaultValue;
        }
    }
}