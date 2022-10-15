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

    public void OnDrag(PointerEventData draggingPoint)      // draggingPoint�� ȭ���� �巡���� �� �Ź� ���ŵ�.
    {
        draggingPos = draggingPoint.position;

        xAngle = (draggingPos.x - beginPos.x) / Screen.width;
        yAngle = (draggingPos.y - beginPos.y) / Screen.height;
        beginPos = draggingPos;     
        // ȭ�鿡�� ���� �����ʰ� �巡�� ������ �ٲٰ� �Ǹ� xAngle, yAngle�� ��ȣ�� �ٲ��� ������ beginPos�� ���� ���� �ʴ� �� ���ŵ��� �ʾ� ������ �߻���.
        // beginPos�� �׻� ���� ��ġ�� �����������ν� �̽� �ذ�. �� y�� ȸ���� �������� ������ �־� MainCamController���� 5�� �����־� �ذ���.
    }
}
