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
	public class PlayerUI : MonoBehaviour, IPlayerReference
	{
        Player player;
		public virtual void Init(Player reference)
        {
            this.player = reference;
        }

        void Update()
        {
            var angles = transform.eulerAngles;
            angles.y = 0f;
            transform.eulerAngles = angles;
        }
    }
}