using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *devYH 카메라 해상도 조절하는 스크립트 (모바일 다양한 해상도 대응 용도)
 */
public class CameraResolution : MonoBehaviour
{
    public Vector2 targetRatio; // 해상도 비율

    // 카메라의 screen.width height 가져와서그걸 카메라 rect에 맞춘다는 의미
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)targetRatio.x / targetRatio.y);
        float scaleWidth = 1f / scaleHeight;
        if (scaleHeight < 1)
        {   
            // 내 기준해상도 보다 더 스마트폰이 좁고 길쭉한 경우
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            // 내 기준 해상도보다 스마트폰이 넓은 경우
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        cam.rect = rect;
    }
}
