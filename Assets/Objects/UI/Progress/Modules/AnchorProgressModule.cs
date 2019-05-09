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
    [RequireComponent(typeof(RectTransform))]
    public class AnchorProgressModule : ProgressBar.Module
	{
        [SerializeField]
        [HideInInspector]
        RectTransform rect;

        [SerializeField]
        protected Slider.Direction direction = Slider.Direction.LeftToRight;
        public Slider.Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;

                UpdateValue();
            }
        }

        protected override void GetDependancies()
        {
            base.GetDependancies();

            rect = GetComponent<RectTransform>();

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
        }

        protected override void SetValue(float value)
        {
            base.SetValue(value);

            switch (direction)
            {
                case Slider.Direction.LeftToRight:
                    {
                        rect.anchorMin = Vector2.zero;
                        rect.anchorMax = new Vector2(value, 1f);
                    }
                    break;

                case Slider.Direction.RightToLeft:
                    {
                        rect.anchorMin = new Vector2(value * -1 + 1, 0f);
                        rect.anchorMax = Vector2.one;
                    }
                    break;

                case Slider.Direction.BottomToTop:
                    {
                        rect.anchorMin = Vector2.zero;
                        rect.anchorMax = new Vector2(1f, value);
                    }
                    break;

                case Slider.Direction.TopToBottom:
                    {
                        rect.anchorMin = new Vector2(0f, value * -1 + 1);
                        rect.anchorMax = Vector2.one;
                    }
                    break;
            }
            
        }
    }
}