using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_roughness;      //��ĥ�� ����
    [SerializeField]
    private float m_magnitude;      //������ ����

    private void Update()
    {
        StartCoroutine(Shake(10f));
    }

    IEnumerator Shake(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * m_roughness;
            transform.position = new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f,
                0f) * m_magnitude * Mathf.PingPong(elapsed, halfDuration);

            yield return null;
        }
    }
}
