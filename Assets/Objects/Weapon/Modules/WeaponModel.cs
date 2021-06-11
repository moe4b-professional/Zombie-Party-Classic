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
    [RequireComponent(typeof(AnimationTriggersRewind))]
	public class WeaponModel : Weapon.Module
	{
        public AnimationTriggersRewind AnimationTriggers { get; protected set; }

        public void Init()
        {
            AnimationTriggers = GetComponent<AnimationTriggersRewind>();
        }
	}
}