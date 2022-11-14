using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_Bullet : MonoBehaviour
{
    int damage;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
            Destroy(gameObject);
        else if (other.gameObject.tag == "BossPartHitbox")
                Debug.Log("충돌처리중:"+other.gameObject.name);
    }
}
