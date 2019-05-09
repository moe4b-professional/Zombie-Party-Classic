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
    [RequireComponent(typeof(Text))]
    public class PopupLabel : MonoBehaviour, Initializer.Interface
    {
        Text label;
        public Text Label { get { return label; } }

        public string Text
        {
            get
            {
                return label.text;
            }
            set
            {
                label.text = value;
            }
        }

        public void Init()
        {
            label = GetComponent<Text>();
        }

        [SerializeField]
        protected float duration = 3f;
        public float Duration { get { return duration; } }

        [SerializeField]
        [Range(0f, 1f)]
        protected float size = 0.7f;
        public float Size { get { return size; } }

        public void Show(string text)
        {
            this.Text = text;

            Show();
        }

        public void Show()
        {
            StopAllCoroutines();

            gameObject.SetActive(true);

            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            var time = 0f;

            while(time != duration)
            {
                time = Mathf.MoveTowards(time, duration, Time.deltaTime);

                transform.localScale = Vector3.one * Mathf.Lerp(size, 1f, time / duration);

                yield return null;
            }

            gameObject.SetActive(false);
        }
	}
}