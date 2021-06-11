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
    public class References
    {
        public static void Init<TReference>(TReference reference)
            where TReference : Component
        {
            Init(reference, Dependancy.GetAll<Interface<TReference>>(reference.gameObject));
        }

        public static void Init<TReference>(TReference reference, IList<Interface<TReference>> targets)
        {
            for (int i = 0; i < targets.Count; i++)
                Init<TReference>(reference, targets[i]);
        }

        public static void Init<TReference>(TReference reference, Interface<TReference> target)
        {
            target.Init(reference);
        }

        public interface Interface<T>
        {
            void Init(T reference);
        }
    }
}