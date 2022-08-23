using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *devYH ī�޶� �ػ� �����ϴ� ��ũ��Ʈ (����� �پ��� �ػ� ���� �뵵)
 */
public class CameraResolution : MonoBehaviour
{
    public Vector2 targetRatio; // �ػ� ����

    // ī�޶��� screen.width height �����ͼ��װ� ī�޶� rect�� ����ٴ� �ǹ�
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)targetRatio.x / targetRatio.y);
        float scaleWidth = 1f / scaleHeight;
        if (scaleHeight < 1)
        {   
            // �� �����ػ� ���� �� ����Ʈ���� ���� ������ ���
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            // �� ���� �ػ󵵺��� ����Ʈ���� ���� ���
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        cam.rect = rect;
    }
}
