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
	public class AnimationTriggersRewind : MonoBehaviour
	{
        Dictionary<Action, string> callbacks = new Dictionary<Action, string>();

        public void Add(Action callback, string ID)
        {
            if (callbacks.ContainsKey(callback)) return;

            callbacks.Add(callback, ID);
        }

        public void Remove(Action callback)
        {
            if (!callbacks.ContainsKey(callback)) return;

            callbacks.Remove(callback);
        }

        void Trigger(string ID)
        {
            foreach (var item in callbacks)
                if (item.Value == ID)
                    item.Key();
        }
    }
}