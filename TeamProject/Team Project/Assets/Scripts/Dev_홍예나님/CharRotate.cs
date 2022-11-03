using CartoonHeroes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CharRotate : MonoBehaviour
{
    public DragOn dragOn;
    private float rotateY;
    private float moveY;

    readonly public float MAXDRAG_Y = 0.6f;
    readonly public float rotateSpeed = 20f;    

    void Move()
    {
        rotateY += dragOn.xAngle * 12f;
        moveY = Mathf.Clamp(moveY + dragOn.yAngle, -MAXDRAG_Y, MAXDRAG_Y);
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;               

        // ī�޶� ȸ��.
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.y - rotateY, 0f) * rotateSpeed);

        // ī�޶� ������.
        Vector3 tmp = transform.position;        
        tmp.y = moveY;
        transform.position = tmp;
    }
    void Zoom()
    {
        float scale = (dragOn.currentDist - dragOn.initialDist) / 1000f;
        Vector3 tmp = transform.position;
        tmp.z = Mathf.Clamp(tmp.z + scale, 1.0f, 2.5f);
        transform.position = tmp;
    }
    
    private void Update()
    {
        if (dragOn.touch.Count == 1) Move();
        else if (dragOn.touch.Count == 2) Zoom();
    }

    // PC������ �� ����� �÷��̾� ������Ʈ�� �̵������ν� ����. (������ ������ �ƴ�.)
    public void SetCharacterZoom(float _scale)
    {
        // �ּ� �ִ� �������� ����.
        _scale = Mathf.Clamp(_scale, 1.0f, 2.5f);
        Vector3 tmp = transform.position;
        tmp.z = _scale;            
        transform.position = tmp;
    }

}
