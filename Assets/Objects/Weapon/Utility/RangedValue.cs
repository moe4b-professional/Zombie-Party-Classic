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
    [Serializable]
	public class RangedValue<T>
	{
		[SerializeField]
        protected T min;
        public T Min { get { return min; } }

        [SerializeField]
        protected T max;
        public T Max { get { return max; } }

        public RangedValue(T min, T max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Serializable]
    public class FloatRangedValue : RangedValue<float>
    {
        public FloatRangedValue(float min, float max) : base(min, max)
        {

        }
    }
}