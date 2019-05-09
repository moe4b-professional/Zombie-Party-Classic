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

using UnityEngine.EventSystems;

namespace Game
{
	public class HoldUIInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
        public bool Value { get; protected set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            Value = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Value = false;
        }
    }
}