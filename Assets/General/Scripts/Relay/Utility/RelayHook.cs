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
	public abstract class RelayHook : MonoBehaviour
	{
        protected virtual void Start()
        {
            var relays = GetComponents<Relay>();

            foreach (var relay in relays)
                relay.Event += Action;
        }

        protected virtual void Action()
        {

        }
	}
}