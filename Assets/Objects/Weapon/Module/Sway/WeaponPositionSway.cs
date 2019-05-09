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
    public class WeaponPositionSway : WeaponSway.Module
    {
        protected override void Reset()
        {
            base.Reset();

            multiplier = new MultiplierData(0.15f, 0.1f);
        }

        public override void ApplyOffset(Vector3 offset)
        {
            if (!enabled)
                return;

            var position = target.localPosition;

            position.x += -offset.x * multiplier.Horizontal;
            position.y += -offset.y * multiplier.Vertical;

            target.localPosition = position;
        }
    }
}