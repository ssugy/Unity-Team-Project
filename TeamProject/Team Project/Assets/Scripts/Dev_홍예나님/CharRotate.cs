using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharRotate : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotationY;
    private Vector3 charPosition;
    //private float dragStartY;
    private const float MAXDRAG_Y = 1.5f;
    private float rotateSpeed = 0.2f;
    private float moveSpeed = 1f;
    private bool isPressed = false;
    private int id1;
    private int id2;
    private bool isZoom = false;
    private Quaternion currentRot;
    private Vector3 currentPos;
    private float initialDist;
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // PC에선 마우스 클릭, 모바일에서는 touch와 같음
       
        if (Input.GetMouseButtonDown(0))
        {
            //UI를 터치하지 않았을 때만 작동
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isPressed = true;
                touchPosition = Input.mousePosition;
                currentRot = transform.rotation;
                currentPos = transform.position;
            }   
        }
        /* 
        if (isPressed && Input.GetMouseButtonDown(1))
        {
            //UI를 터치하지 않았을 때만 작동
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isZoom = true;
                touchPosition = Input.mousePosition;
                currentRot = transform.rotation;
                currentPos = transform.position;
            }
        }
        */


#else
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                isPressed = true;
                id1 = touch.fingerId;
                touchPosition = touch.position; //Input.mousePosition;
                currentRot = transform.rotation;
                currentPos = transform.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                isPressed = false;
            }

            if (Input.touchCount > 1)
            {
                touch = Input.GetTouch(1);
                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !isZoom)
                {
                    isZoom = true;
                    id2 = touch.fingerId;
                    var first = Input.GetTouch(0);
                    var second = Input.GetTouch(1);
                    initialDist = Vector2.Distance(first.position, second.position);
                }
            }
            else
            {
                isZoom = false;
            }
        }
        else
        {
            isPressed = false;
            isZoom = false;
        }
#endif
        //if(Input.touchCount > 0)
        //    touch = Input.GetTouch(0);
        //if (touch.phase == TouchPhase.Moved)
        if (isPressed)
        {
            //rotationY = Quaternion.Euler(0f, - touch.deltaPosition.x * rotateSpeed, 0f);
            rotationY = Quaternion.Euler(0f, -(Input.mousePosition.x - touchPosition.x ) * rotateSpeed, 0f);
            charPosition = currentPos;
            charPosition.y += (Input.mousePosition.y - touchPosition.y) / Screen.height * moveSpeed;
            charPosition.y = Mathf.Clamp(charPosition.y, -MAXDRAG_Y, MAXDRAG_Y);
            transform.rotation = rotationY * currentRot;
            transform.position = charPosition;
        }
        if (isZoom && initialDist > 0f)
        {
            if (Input.touchCount >= 2)
            {
                var first = Input.GetTouch(0);
                var second = Input.GetTouch(1);
                float dist = Vector2.Distance(first.position, second.position);
                float scale = dist / initialDist;
                scale = Mathf.Clamp(scale, 0.5f, 2.0f) - 1.5f;
                charPosition.z = scale;
                //transform.localScale = new Vector3(scale, scale, scale);
                transform.position = charPosition;
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
#endif
    }
}
