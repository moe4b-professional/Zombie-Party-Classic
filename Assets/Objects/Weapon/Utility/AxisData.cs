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
    [Serializable]
	public class AxisData<T>
    {
        [SerializeField]
        protected T vertical;
        public T Vertical { get { return vertical; } }

        [SerializeField]
        protected T horizontal;
        public T Horizontal { get { return horizontal; } }

        public AxisData(T vertical, T horizontal)
        {
            this.vertical = vertical;
            this.horizontal = horizontal;
        }
    }

    [Serializable]
    public class FloatAxisData : AxisData<float>
    {
        public FloatAxisData(float vertical, float horizontal) : base(vertical, horizontal)
        {

        }
    }
}