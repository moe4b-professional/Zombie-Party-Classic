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
    [RequireComponent(typeof(Image))]
	public class ScreenFade : MonoBehaviour
	{
        [SerializeField]
        protected float speed = 5;
        public float Speed { get { return speed; } }

        Image image;

        public float Transparency
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

                image.raycastTarget = Transparency != 0f;
            }
        }

        public virtual void Init(float value)
        {
            image = GetComponent<Image>();

            gameObject.SetActive(true);

            Transparency = value;
        }
        public virtual void Init(float value, float target)
        {
            Init(value);

            Transition(target);
        }

        public event Action OnTransitionEnd;
        public virtual void Transition(float target)
        {
            if (Coroutine != null) StopCoroutine(Coroutine);

            Coroutine = StartCoroutine(Procedure(target));
        }
        public virtual void Transition(float target, float initialValue)
        {
            this.Transparency = target;

            Transition(initialValue);
        }

        public Coroutine Coroutine { get; protected set; }
        IEnumerator Procedure(float target)
        {
            while(Transparency != target)
            {
                Transparency = Mathf.MoveTowards(Transparency, target, speed * Time.unscaledDeltaTime);

                yield return null;
            }

            Coroutine = null;

            if (OnTransitionEnd != null) OnTransitionEnd();
        }
    }
}