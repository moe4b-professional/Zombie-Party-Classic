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
	public class UITransitionResize : UITransition.Module
	{
		[SerializeField]
        protected float minSize = 0f;
        public float MinSize { get { return minSize; } }

        [SerializeField]
        protected float maxSize = 100;
        public float MaxSize
        {
            get
            {
                return maxSize;
            }
            set
            {
                if (value < minSize)
                    maxSize = value;

                maxSize = value;
            }
        }

        [SerializeField]
        protected AxisTarget axis;
        public AxisTarget Axis { get { return axis; } }
        public enum AxisTarget { X, Y };

        RectTransform rect;

        public override void Init()
        {
            rect = GetComponent<RectTransform>();

            base.Init();

            transition.OnValueChanged += OnValueChanged;
        }

        protected override void UpdateState(float value)
        {
            base.UpdateState(value);

            var size = rect.sizeDelta;

            if (axis == AxisTarget.X) size.x = Mathf.Lerp(minSize, maxSize, value);
            if (axis == AxisTarget.Y) size.y = Mathf.Lerp(minSize, maxSize, value);

            rect.sizeDelta = size;
        }
    }
}