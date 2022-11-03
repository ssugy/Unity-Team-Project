using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public float xAngle;    
    [HideInInspector] public float yAngle;

    // ��ġ�� �������� �� ���޵Ǵ� PointerEventData �ν��Ͻ��� �巡���� ��, ��ġ�� ���� ������ ���� �ν��Ͻ���� ���� ����.
    // ó������ ���� ��ġ �Է¸��� ����Ͽ� ȭ�� ȸ��.
    public List<PointerEventData> touch = new List<PointerEventData>();
    public Vector3 beginPos;
    public Vector3 draggingPos;

    public Vector3 secondBeginPos;  // ī�޶� �ܿ� ���.
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

    public void OnDrag(PointerEventData draggingPoint)      // draggingPoint�� ȭ���� �巡���� �� �Ź� ���ŵ�.
    {
        if (touch.Count == 0)
        {
            touch.Add(draggingPoint);
            beginPos = touch[0].position;
        }
        draggingPos = touch[0].position;
        xAngle = (draggingPos.x - beginPos.x) / Screen.width * 1.8f;
        yAngle = (draggingPos.y - beginPos.y) / Screen.height * 1.8f;
        
            // ȭ�鿡�� ���� �����ʰ� �巡�� ������ �ٲٰ� �Ǹ� xAngle, yAngle�� ��ȣ�� �ٲ��� ������ beginPos�� ���� ���� �ʴ� �� ���ŵ��� �ʾ� ������ �߻���.
            // beginPos�� �׻� ���� ��ġ�� �����������ν� �̽� �ذ�. �� y�� ȸ���� �������� ������ �־� MainCamController���� 5�� �����־� �ذ���.       
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
