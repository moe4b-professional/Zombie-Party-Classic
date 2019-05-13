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
	public class UITransitionDynamicResize : UITransitionResize
	{
        [SerializeField]
        protected RectTransform panel;
        public RectTransform Panel { get { return panel; } }
        public float PanelSize
        {
            get
            {
                switch (axis)
                {
                    case AxisTarget.X:
                        return panel.sizeDelta.x;

                    case AxisTarget.Y:
                        return panel.sizeDelta.y;
                }

                throw new NotImplementedException();
            }
        }

        protected virtual void Reset()
        {
            var sizeFitter = Dependancy.Get<ContentSizeFitter>(gameObject);

            if (sizeFitter != null)
            {
                panel = sizeFitter.transform as RectTransform;

                if (sizeFitter.verticalFit == ContentSizeFitter.FitMode.PreferredSize) axis = AxisTarget.Y;
                else if (sizeFitter.horizontalFit == ContentSizeFitter.FitMode.PreferredSize) axis = AxisTarget.X;

                MaxSize = PanelSize;
            }
        }

        public override void Init()
        {
            if (panel == null)
                throw new NullReferenceException();

            base.Init();
        }

        protected override void UpdateState(float value)
        {
            MaxSize = PanelSize;

            base.UpdateState(value);
        }
    }
}