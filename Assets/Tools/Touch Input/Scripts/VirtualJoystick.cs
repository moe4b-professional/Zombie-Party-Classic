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

public class VirtualJoystick : AxisInput, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public IPointerDownHandler PointerDownHandler { get { return this; } }
    public IDragHandler DragHandler { get { return this; } }
    public IPointerUpHandler PointerUpHandler { get { return this; } }

    [SerializeField]
    VirtualJoystickRig rig;
    public VirtualJoystickRig Rig { get { return rig; } }

    [SerializeField]
    bool snap = true;
    public bool Snap
    {
        get
        {
            return snap;
        }
        set
        {
            snap = value;
        }
    }
    Vector2 startPosition;

    [SerializeField]
    bool autoHide = false;
    public bool AutoHide
    {
        get
        {
            return autoHide;
        }
        set
        {
            autoHide = value;
        }
    }

    protected virtual void Awake()
    {
        startPosition = rig.Background.rectTransform.position;

        if (autoHide)
            Hide();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Show();

        if (snap)
            rig.BackgroundRect.position = eventData.position;

        DragHandler.OnDrag(eventData);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rig.KnobRect.position = eventData.position;

        if(rig.KnobRect.localPosition.magnitude > rig.BackgroundRect.sizeDelta.x / 2)
            rig.KnobRect.localPosition = rig.KnobRect.localPosition.normalized * rig.BackgroundRect.sizeDelta.x / 2;

        SetValue(rig.KnobRect.localPosition / rig.BackgroundRect.sizeDelta.x * 2f);
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        rig.BackgroundRect.position = startPosition;
        rig.KnobRect.localPosition = Vector3.zero;

        SetValue(Vector2.zero);

        if (autoHide)
            Hide();
    }

    protected virtual void Show()
    {
        SetVisibility(true);
    }
    protected virtual void Hide()
    {
        SetVisibility(false);
    }
    protected virtual void SetVisibility(bool value)
    {
        rig.Background.enabled = value;
        rig.Knob.enabled = value;
    }
}

[Serializable]
public struct VirtualJoystickRig
{
    [SerializeField]
    Image background;
    public Image Background { get { return background; } }
    public RectTransform BackgroundRect { get { return background.rectTransform; } }

    [SerializeField]
    Image knob;
    public Image Knob { get { return knob; } }
    public RectTransform KnobRect { get { return knob.rectTransform; } }
}