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

using System.Text.RegularExpressions;

namespace Game
{
	public static class Mimes
	{
        public static void Configure()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "Mimes.dat");

            if (File.Exists(path))
                List = Load(path);
            else
                Debug.LogError("Can't Find A Mime file, expected path: " + path);
        }

        public static List<Data> List { get; private set; }

        public static List<Data> Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Mime File Not Found", path);

            var list = new List<Data>();

            var lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    list.Add(Parse(lines[i]));
                }
                catch (InvalidDataException)
                {
                    continue;
                }
            }

            return list;
        }

        public static Data Parse(string text)
        {
            if (Regex.Matches(text, " : ").Count != 1)
                throw new InvalidDataException("Cannot Parse: " + text + " as a Mime");

            var parts = Regex.Split(text, " : ");

            return new Data(parts[0], parts[1]);
        }

        public static Data FindByExtension(string extension)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (CaseInsensitiveCompare(List[i].Extension, extension))
                    return List[i];
            }

            throw new ArgumentException("No Mime extension: " + extension + " was registered");
        }

        public static Data FindByValue(string value)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (CaseInsensitiveCompare(List[i].Value, value))
                    return List[i];
            }

            throw new ArgumentException("No Mime value: " + value + " was registered");
        }

        public static bool CaseInsensitiveCompare(string value1, string value2)
        {
            return value1.ToLower() == value2.ToLower();
        }

        [Serializable]
        public struct Data
        {
            public string Extension { get; private set; }
            public string Value { get; private set; }

            public Data(string extension, string value)
            {
                this.Extension = extension;
                this.Value = value;
            }
        }
	}
}