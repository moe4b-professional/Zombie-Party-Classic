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
	[RequireComponent(typeof(ConstraintArea))]
	public class SetCameraConstraintArea : MonoBehaviour
	{
		[SerializeField]
		float padding = 1f;

		Camera Camera;

		ConstraintArea Area;

		void Start()
		{
			Camera = Camera.main;
			Area = GetComponent<ConstraintArea>();

			Area.Width = CalculateWidth() - padding;
			Area.Length = CalculateLength() - padding;
		}

		float CalculateWidth()
		{
			var p1 = Camera.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.transform.position.y));
			var p2 = Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, Camera.transform.position.y));

			return Vector3.Distance(p1, p2);
		}

		float CalculateLength()
        {
			var p1 = Camera.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.transform.position.y));
			var p2 = Camera.ScreenToWorldPoint(new Vector3(0f, Screen.height, Camera.transform.position.y));

			return Vector3.Distance(p1, p2);
		}
	}
}