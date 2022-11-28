using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFire : MonoBehaviour
{

    // 파이어볼은 방어율을 무시하고 고정적인 100 데미지를 입힌다.
    public int damage;

    private void OnEnable()
    {
        Invoke("ActiveFalseSelf", 4f);
    }
    void ActiveFalseSelf()
    {
        gameObject.SetActive(false);
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
