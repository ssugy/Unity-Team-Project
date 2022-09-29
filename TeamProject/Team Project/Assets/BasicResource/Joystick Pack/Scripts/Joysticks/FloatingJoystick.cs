using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    // 본래 FixedJoystick 스크립트를 커스텀하여 Floating 타입으로 수정하였음.
    public static FloatingJoystick instance;
    public Transform joystick;
    public Vector3 initial;

    private void Awake()
    {
        instance = this;
        initial = joystick.position;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        joystick.position = eventData.position;
        OnDrag(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        joystick.position = initial;
    }

}