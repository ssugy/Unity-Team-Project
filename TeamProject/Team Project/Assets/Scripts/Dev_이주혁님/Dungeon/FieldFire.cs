using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFire : MonoBehaviour
{

    // ���̾�� ������� �����ϰ� �������� 100 �������� ������.
    public int damage;
    float t = 0f;

    private void OnEnable()
    {
        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        t += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.IsAttacked(damage, null);
            t = 0f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (t >= 0.5f)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                    player.IsAttacked(damage, null);
                t -= 0.5f;
            }                       
        }
    }

}
