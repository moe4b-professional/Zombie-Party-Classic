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
	public class FillProgressModule : ProgressBar.Module
	{
        [SerializeField]
        [HideInInspector]
        Image image;

        protected override void Reset()
        {
            base.Reset();

            image.fillMethod = Image.FillMethod.Radial360;

            image.fillOrigin = 2;
        }

        protected override void GetDependancies()
        {
            base.GetDependancies();

            image = GetComponent<Image>();

            image.type = Image.Type.Filled;
        }

        protected override void SetValue(float value)
        {
            base.SetValue(value);

            image.fillAmount = value;

            if (!Application.isPlaying)
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }
    }
}