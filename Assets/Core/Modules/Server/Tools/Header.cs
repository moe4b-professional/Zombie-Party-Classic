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
    public static class Header
    {
        public static bool IsValid(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            int count = text.Count(character => character == ':');

            if (count > 1) return false;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ':')
                {
                    if (i == 0) return false;
                    else if (i == text.Length - 1) return false;
                    else return true;
                }
            }

            return false;
        }

        public static KeyValuePair<string, string> Parse(string text)
        {
            if (!IsValid(text))
                throw new InvalidDataException();

            text = Regex.Replace(text, ": ", ":");

            var pair = text.Split(':');

            pair[0] = pair[0].ToLower();

            return new KeyValuePair<string, string>(pair[0], pair[1]);
        }

        public static Dictionary<string, string> ParseAll(IList<string> headers)
        {
            var dictionary = new Dictionary<string, string>();

            for (int i = 0; i < headers.Count; i++)
            {
                var pair = Parse(headers[i]);

                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }

        public static KeyValuePair<string, string> Find(string key, Dictionary<string, string> headers)
        {
            foreach (var pair in headers)
                if (pair.Key.ToLower() == key.ToLower())
                    return pair;

            throw new KeyNotFoundException("No header with the key: " + key + " was found");
        }
    }
}