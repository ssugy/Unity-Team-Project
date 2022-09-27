using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharRotate : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotationY;
    private float rotateSpeed = 0.01f;
    private bool isPressed = false;

    // Update is called once per frame
    void Update()
    {
        // PC���� ���콺 Ŭ��, ����Ͽ����� touch�� ����
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;

            //UI�� ��ġ���� �ʾ��� ���� �۵�
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isPressed = true;
            }   
        }
        //if(Input.touchCount > 0)
        //    touch = Input.GetTouch(0);
        //if (touch.phase == TouchPhase.Moved)
        if (isPressed)
        {
            //rotationY = Quaternion.Euler(0f, - touch.deltaPosition.x * rotateSpeed, 0f);
            rotationY = Quaternion.Euler(0f, (Input.mousePosition.x - touchPosition.x ) * rotateSpeed, 0f);
            transform.rotation = rotationY * transform.rotation;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
    }
}
