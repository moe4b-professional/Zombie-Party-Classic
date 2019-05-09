using System;
using System.IO;
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

public class AxisInput : MonoBehaviour
{
    [SerializeField]
    protected Vector2 value;
    public Vector2 Value { get { return value; } }

    [SerializeField]
    protected float sensitivity = 1f;
    public float Sensitivity
    {
        get
        {
            return sensitivity;
        }
        set
        {
            sensitivity = value;
        }
    }

    [SerializeField]
    protected InvertAxis invert;
    public InvertAxis Invert
    {
        get
        {
            return invert;
        }
        set
        {
            invert = value;
        }
    }

    public event Action<Vector2> OnSetValue;
    protected virtual void SetValue(Vector2 newValue)
    {
        newValue = EditValue(newValue);

        newValue = ApplyModifiers(newValue);

        value = newValue;

        if (OnSetValue != null) OnSetValue(value);
    }

    protected virtual Vector2 EditValue(Vector2 newValue)
    {
        return newValue;
    }

    protected virtual Vector2 ApplyModifiers(Vector2 newValue)
    {
        newValue *= sensitivity;

        newValue = invert.GetAxis(newValue);

        return newValue;
    }
}

[Serializable]
public struct InvertAxis
{
    [SerializeField]
    bool x;
    public bool X { get { return x; } }
    public float XScale { get { return x ? -1f : 1f; } }

    [SerializeField]
    bool y;
    public bool Y { get { return y; } }
    public float YScale { get { return y ? -1f : 1f; } }

    public InvertAxis(bool x, bool y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 GetAxis(Vector2 axis)
    {
        axis.x *= XScale;
        axis.y *= YScale;

        return axis;
    }
}