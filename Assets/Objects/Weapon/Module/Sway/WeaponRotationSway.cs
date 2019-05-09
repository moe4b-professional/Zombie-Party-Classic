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
    public class WeaponRotationSway : WeaponSway.Module
    {
        [SerializeField]
        float turnMultiplier = 2f;

        protected override void Reset()
        {
            base.Reset();

            multiplier = new MultiplierData(20f, 10f);
        }

        public override void ApplyOffset(Vector3 offset)
        {
            if (!enabled)
                return;

            var angles = target.localEulerAngles;

            angles.y += offset.x * multiplier.Horizontal;
            angles.z = angles.y * turnMultiplier;

            angles.x += -offset.y * multiplier.Vertical;

            target.localEulerAngles = angles;
        }
    }
}