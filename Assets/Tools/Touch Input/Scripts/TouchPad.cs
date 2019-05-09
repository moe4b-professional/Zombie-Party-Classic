using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TouchPad : AxisInput, IDragHandler
{
    Vector2? delta;

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        delta = eventData.delta;
    }

    protected virtual void Update()
    {
        SetValue(delta.HasValue ? delta.Value : Vector2.zero);
        delta = null;
    }

    public TouchPad()
    {
        sensitivity = 0.1f;
    }
}