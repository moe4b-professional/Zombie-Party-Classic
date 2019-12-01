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