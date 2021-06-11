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
	public class TopScoreEntryUITemplate : UIElement
	{
		[SerializeField]
        Text label = default;

        public ScoresCore.Entry Data { get; protected set; }

		public void Set(ScoresCore.Entry item)
        {
			Data = item;

			label.text = $"{item.Name}: {item.Value}";
		}
	}
}