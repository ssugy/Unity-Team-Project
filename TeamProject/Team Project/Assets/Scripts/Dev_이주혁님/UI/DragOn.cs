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
    List<PointerEventData> touch = new List<PointerEventData>();
    Vector3 beginPos;
    Vector3 draggingPos;

    public void OnBeginDrag(PointerEventData beginPoint)
    {
        if (touch.Count == 0)
        {
            touch.Add(beginPoint);
            beginPos = touch[0].position;
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
        xAngle = (draggingPos.x - beginPos.x) / Screen.width;
        yAngle = (draggingPos.y - beginPos.y) / Screen.height;
        beginPos = draggingPos;     
        // ȭ�鿡�� ���� �����ʰ� �巡�� ������ �ٲٰ� �Ǹ� xAngle, yAngle�� ��ȣ�� �ٲ��� ������ beginPos�� ���� ���� �ʴ� �� ���ŵ��� �ʾ� ������ �߻���.
        // beginPos�� �׻� ���� ��ġ�� �����������ν� �̽� �ذ�. �� y�� ȸ���� �������� ������ �־� MainCamController���� 5�� �����־� �ذ���.
    }
    public void OnEndDrag(PointerEventData endPoint)
    {
        touch.Remove(endPoint);
    }
}
