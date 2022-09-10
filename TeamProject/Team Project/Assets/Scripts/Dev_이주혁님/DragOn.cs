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

    public void OnDrag(PointerEventData draggingPoint)
    {
        draggingPos = draggingPoint.position;

        xAngle = (draggingPos.x - beginPos.x) / Screen.width;
        yAngle = (draggingPos.y - beginPos.y) / Screen.height;        
    }
}
