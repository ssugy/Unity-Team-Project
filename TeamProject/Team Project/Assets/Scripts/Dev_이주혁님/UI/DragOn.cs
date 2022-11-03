using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public float xAngle;    
    [HideInInspector] public float yAngle;

    // 터치를 시작했을 때 전달되는 PointerEventData 인스턴스는 드래깅할 때, 터치를 끝낼 때까지 같은 인스턴스라는 점에 착안.
    // 처음으로 받은 터치 입력만을 사용하여 화면 회전.
    public List<PointerEventData> touch = new List<PointerEventData>();
    public Vector3 beginPos;
    public Vector3 draggingPos;

    public Vector3 secondBeginPos;  // 카메라 줌에 사용.
    public Vector3 secondDraggingPos;

    public float initialDist;
    public float currentDist;

    public void OnBeginDrag(PointerEventData beginPoint)
    {
        if (touch.Count == 0)
        {
            touch.Add(beginPoint);
            beginPos = touch[0].position;
        }     
        else if (touch.Count == 1)
        {
            touch.Add(beginPoint);
            beginPos = touch[0].position;
            secondBeginPos = touch[1].position;
        }
    }

    public void OnDrag(PointerEventData draggingPoint)      // draggingPoint는 화면을 드래그할 때 매번 갱신됨.
    {
        if (touch.Count == 0)
        {
            touch.Add(draggingPoint);
            beginPos = touch[0].position;
        }
        draggingPos = touch[0].position;
        xAngle = (draggingPos.x - beginPos.x) / Screen.width * 1.8f;
        yAngle = (draggingPos.y - beginPos.y) / Screen.height * 1.8f;
        
            // 화면에서 손을 떼지않고 드래그 방향을 바꾸게 되면 xAngle, yAngle의 부호가 바뀌어야 하지만 beginPos는 손을 뗴지 않는 한 갱신되지 않아 문제가 발생함.
            // beginPos를 항상 이전 위치로 갱신해줌으로써 이슈 해결. 단 y축 회전이 느려지는 문제가 있어 MainCamController에서 5를 곱해주어 해결함.       
        if (touch.Count == 2)
        {            
            secondDraggingPos = touch[1].position;
            initialDist = Vector3.Distance(beginPos, secondBeginPos);
            currentDist = Vector3.Distance(draggingPos, secondDraggingPos);
            secondBeginPos = secondDraggingPos;
        }
        beginPos = draggingPos;

    }
    public void OnEndDrag(PointerEventData endPoint)
    {
        touch.Remove(endPoint);
    }
}
