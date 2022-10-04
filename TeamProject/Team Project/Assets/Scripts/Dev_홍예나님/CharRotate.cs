using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharRotate : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotationY;
    private float rotateSpeed = 0.2f;
    private bool isPressed = false;
    private Quaternion currentRot;

    // Update is called once per frame
    void Update()
    {
        // PC에선 마우스 클릭, 모바일에서는 touch와 같음
        if (Input.GetMouseButtonDown(0))
        {
            

            //UI를 터치하지 않았을 때만 작동
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isPressed = true;
                touchPosition = Input.mousePosition;
                currentRot = transform.rotation;
            }   
        }
        //if(Input.touchCount > 0)
        //    touch = Input.GetTouch(0);
        //if (touch.phase == TouchPhase.Moved)
        if (isPressed)
        {
            //rotationY = Quaternion.Euler(0f, - touch.deltaPosition.x * rotateSpeed, 0f);
            rotationY = Quaternion.Euler(0f, -(Input.mousePosition.x - touchPosition.x ) * rotateSpeed, 0f);
            transform.rotation = rotationY * currentRot;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
    }
}
