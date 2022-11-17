using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_ShakeCamera : MonoBehaviour
{
    public float shakeTime;
    public float shakeIntensity;
    public float delay;
    bool isShake;
    public void onSakeCamera()
    {
        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }
    IEnumerator ShakeByPosition()
    {
        yield return new WaitForSeconds(delay);
        isShake = true;
    }
    
    private void FixedUpdate()
    {
        if (isShake)
        {
            /*float tmp = shakeTime;
            while (tmp > 0.0f)
            {
                float cameraShakeY = Random.Range(0f, 1f) * shakeIntensity;
                Vector3 moveVec = new Vector3(0f,cameraShakeY,0f);
                Vector3 tmpVec = transform.position + moveVec;
                transform.position = tmpVec;
                tmp -= Time.deltaTime;
            }
            isShake = false;*/
        }
    }
}
