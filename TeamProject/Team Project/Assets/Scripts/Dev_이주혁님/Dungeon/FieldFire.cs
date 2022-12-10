using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFire : MonoBehaviour
{

    // ���̾�� ������� �����ϰ� �������� 100 �������� ������.
    public int damage;

    private void OnEnable()
    {
        Invoke("DestroySelf", 4f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.IsAttacked(damage, null);
        }
    }

}
