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
    public PointerEventData eventData;

    private void Awake()
    {
        instance = this;
        initial = joystick.localPosition;
    }
    public override void OnPointerDown(PointerEventData _eventData)
    {
        eventData = _eventData;
        joystick.position = _eventData.position;
        OnDrag(_eventData);
    }
    public override void OnPointerUp(PointerEventData _eventData)
    {        
        // 현재 이벤트 데이터가 인식하고 있는, 드래그 중인 오브젝트를 null로 바꿔줌.
        // 드래그 중인 오브젝트가 없으므로 OnDrag가 발생하지 않음.
        _eventData.pointerDrag = null;
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        joystick.localPosition = initial;
    }
        
}