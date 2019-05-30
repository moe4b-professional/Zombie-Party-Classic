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
        public const char Colon = ':';

        public static bool IsValid(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            if (text.Count(character => character == Colon) < 1) return false; //Make sure there is only one colon in the text

            var index = text.IndexOf(Colon); //get the index of that semi colon
            if (index == 0 || index == text.Length - 1) return false; //return false if the semi colon is the first or last character

            return true;
        }

        public static KeyValuePair<string, string> Parse(string text)
        {
            if (!IsValid(text))
                throw new InvalidDataException();

            var index = text.IndexOf(Colon);

            var key = text.Substring(0, index).Trim();
            var value = text.Substring(index + 1).Trim();

            return new KeyValuePair<string, string>(key, value);
        }

        public static Dictionary<string, string> ParseAll(IList<string> headers)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headers.Count; i++)
            {
                var pair = Parse(headers[i]);

                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }
        public static Dictionary<string, string> ParseAll(string text)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var lines = Regex.Split(text, Environment.NewLine);

            for (int i = 0; i < lines.Length; i++)
            {
                if (!IsValid(lines[i])) continue;

                var element = Parse(lines[i]);

                dictionary.Add(element.Key, element.Value);
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

        public static bool Compare(KeyValuePair<string, string> header, string key)
        {
            return header.Key == key.ToLower();
        }
    }
}