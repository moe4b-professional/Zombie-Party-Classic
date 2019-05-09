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
    public class CoroutineManager
    {
        MonoBehaviour behaviour;

        public Coroutine Start(Func<IEnumerator> function)
        {
            return behaviour.StartCoroutine(function());
        }

        public Coroutine YieldTime(Action action, float delay)
        {
            return behaviour.StartCoroutine(YieldTimeProcedure(action, delay));
        }
        IEnumerator YieldTimeProcedure(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);

            action();
        }

        public Coroutine YieldFrame(Action action)
        {
            return behaviour.StartCoroutine(YieldFrameProcedure(action));
        }
        IEnumerator YieldFrameProcedure(Action action)
        {
            yield return new WaitForEndOfFrame();

            action();
        }

        public CoroutineManager(MonoBehaviour behaviour)
        {
            this.behaviour = behaviour;
        }
    }
}