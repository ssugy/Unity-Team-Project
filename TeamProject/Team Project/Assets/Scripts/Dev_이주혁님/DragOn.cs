using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOn : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [HideInInspector] public float xAngle;    
    [HideInInspector] public float yAngle;    

    Vector3 beginPos;
    Vector3 draggingPos;

    public void OnBeginDrag(PointerEventData beginPoint)
    {
        beginPos = beginPoint.position;        
    }

    public void OnDrag(PointerEventData draggingPoint)      // draggingPoint는 화면을 드래그할 때 매번 갱신됨.
    {
        draggingPos = draggingPoint.position;

        xAngle = (draggingPos.x - beginPos.x) / Screen.width;
        yAngle = (draggingPos.y - beginPos.y) / Screen.height;
        beginPos = draggingPos;     
        // 화면에서 손을 떼지않고 드래그 방향을 바꾸게 되면 xAngle, yAngle의 부호가 바뀌어야 하지만 beginPos는 손을 뗴지 않는 한 갱신되지 않아 문제가 발생함.
        // beginPos를 항상 이전 위치로 갱신해줌으로써 이슈 해결. 단 y축 회전이 느려지는 문제가 있어 MainCamController에서 5를 곱해주어 해결함.
    }
}
