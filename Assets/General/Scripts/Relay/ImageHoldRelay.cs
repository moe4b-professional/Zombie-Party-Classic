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

using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class ImageHoldRelay : Relay<Image>, IPointerDownHandler, IPointerUpHandler
    {
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            coroutine = StartCoroutine(Procedure());
        }

        Coroutine coroutine;
        public bool IsClicked { get { return coroutine != null; } }

        protected virtual IEnumerator Procedure()
        {
            while(true)
            {
                yield return new WaitForEndOfFrame();

                InvokeEvent();
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            Stop();
        }

        void OnDisable()
        {
            Stop();
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}