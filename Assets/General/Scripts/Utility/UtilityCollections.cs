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
    public class Glossary<TKey, TValue>
    {
        public Dictionary<TValue, TKey> Keys { get; protected set; }
        public Dictionary<TKey, TValue> Values { get; protected set; }

        public TValue this[TKey key] => Values[key];
        public TKey this[TValue value] => Keys[value];

        public virtual void Add(TKey key, TValue value)
        {
            Values.Add(key, value);
            Keys.Add(value, key);
        }

        public virtual void Set(TKey key, TValue value)
        {
            Values[key] = value;
            Keys[value] = key;
        }

        public virtual bool TryGetKey(TValue value, out TKey key) => Keys.TryGetValue(value, out key);
        public virtual bool TryGetValue(TKey key, out TValue value) => Values.TryGetValue(key, out value);

        public virtual bool Contains(TKey key) => Values.ContainsKey(key);
        public virtual bool Contains(TValue value) => Keys.ContainsKey(value);

        public virtual void Remove(TKey key)
        {
            if (Values.TryGetValue(key, out var value) == false)
            {
                Values.Remove(key);
                return;
            }

            Keys.Remove(value);
        }
        public virtual void Remove(TValue value)
        {
            if (Keys.TryGetValue(value, out var key) == false)
            {
                Keys.Remove(value);
                return;
            }

            Values.Remove(key);
        }

        public Glossary()
        {
            Values = new Dictionary<TKey, TValue>();

            Keys = new Dictionary<TValue, TKey>();
        }
    }
}