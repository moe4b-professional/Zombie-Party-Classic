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
    [RequireComponent(typeof(Text))]
	public class PlayerNameLabel : MonoBehaviour, IPlayerReference
	{
        Text label;
        public Color Color
        {
            get
            {
                return label.color;
            }
            set
            {
                label.color = value;
            }
        }
        public float Transparency
        {
            get
            {
                return Color.a;
            }
            set
            {
                var tempColor = Color;

                tempColor.a = value;

                Color = tempColor;
            }
        }

        [SerializeField]
        protected float speed = 4f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float duration = 5f;
        public float Duration { get { return duration; } }

        [SerializeField]
        float target = 0.5f;
        public float Target => target;

        public Level Level { get { return Level.Instance; } }

        Player player;
        public virtual void Init(Player reference)
        {
            this.player = reference;

            label = GetComponent<Text>();
            label.text = reference.Client.Name;
            gameObject.SetActive(true);
            Transparency = 0f;

            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            yield return Transition(1f);

            yield return new WaitForSeconds(duration);

            yield return Transition(target);
        }

        IEnumerator Transition(float target)
        {
            while(Transparency != target)
            {
                Transparency = Mathf.MoveTowards(Transparency, target, speed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
        }
    }
}