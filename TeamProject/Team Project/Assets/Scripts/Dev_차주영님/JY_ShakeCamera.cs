using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_ShakeCamera : MonoBehaviour
{
    public float shakeTime;
    public float shakeIntensity;
    public float delay;
    public void onSakeCamera()
    {
        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }
    IEnumerator ShakeByPosition()
    {
        yield return new WaitForSeconds(delay);
        Vector3 startposition = transform.position;
        float tmp = shakeTime;
        while (tmp > 0.0f)
        {
            transform.position = startposition + Random.insideUnitSphere * shakeIntensity;
            tmp -= Time.deltaTime;
            yield return null;
        }
        transform.position = startposition;
    }
}
