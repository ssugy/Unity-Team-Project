using CartoonHeroes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharRotate : MonoBehaviour
{    
    readonly public float MAXDRAG_Y = 1.5f;
    readonly public float rotateSpeed = 0.2f;
    readonly public float moveSpeed = 1f;

    //private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotationY;
    private Vector3 charPosition;
    //private float dragStartY;
    
    private bool isPressed = false;
    private int id1;
    private int id2;
    private bool isZoom = false;
    private Quaternion currentRot;
    private Vector3 currentPos;
    private float initialDist;

    void Update()
    {        
        
        // PC 기준.
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            // 이벤트 시스템 오브젝트(UI)를 터치하지 않았을 때만 작동
            if (!EventSystem.current.IsPointerOverGameObject())
            {                
                isPressed = true;
                touchPosition = Input.mousePosition;
                currentRot = transform.rotation;
                currentPos = transform.position;
            }   
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
        // 모바일 기준.
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
                isZoom = false;                
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
#endif      // if 종료.

        if (isPressed)
        {            
            rotationY = Quaternion.Euler(0f, -(Input.mousePosition.x - touchPosition.x) * rotateSpeed, 0f);
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
                SetCharacterZoom(scale);
            }
        }

    }

    // 줌 기능은 플레이어 오브젝트를 이동함으로써 구현. (스케일 변경이 아님.)
    public void SetCharacterZoom(float _scale)
    {
        // 최소 최대 스케일을 지정.
        _scale = Mathf.Clamp(_scale, 1.0f, 2.5f);
        Vector3 tmp = transform.position;
        tmp.z = _scale;
        //charPosition.x = _scale * -0.15f - 0.575f;        
        transform.position = tmp;
    }

}
