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
    [RequireComponent(typeof(Image))]
	public class ScreenFade : MonoBehaviour
	{
        [SerializeField]
        protected float speed = 5;
        public float Speed { get { return speed; } }

        Image image;

        public float Alpha
        {
            get
            {
                return image.color.a;
            }
            set
            {
                var color = image.color;
                color.a = value;
                image.color = color;

                image.raycastTarget = Alpha != 0f;
            }
        }

        public virtual void Init(float value)
        {
            image = GetComponent<Image>();

            gameObject.SetActive(true);

            Alpha = value;
        }

        public event Action OnTransitionEnd;
        public virtual Coroutine Transition(float target)
        {
            if (Coroutine != null) StopCoroutine(Coroutine);

            Coroutine = StartCoroutine(Procedure(target));

            return Coroutine;
        }

        public Coroutine Coroutine { get; protected set; }
        IEnumerator Procedure(float target)
        {
            while(Alpha != target)
            {
                Alpha = Mathf.MoveTowards(Alpha, target, speed * Time.unscaledDeltaTime);

                yield return new WaitForEndOfFrame();
            }

            Coroutine = null;

            if (OnTransitionEnd != null) OnTransitionEnd();
        }
    }
}