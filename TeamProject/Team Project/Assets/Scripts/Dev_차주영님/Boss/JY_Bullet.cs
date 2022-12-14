using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_Bullet : MonoBehaviour
{    
    private void OnEnable()
    {
        Destroy(gameObject, 3f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "BossPartHitbox")
        {
            JY_Boss_FireDungeon.s_instance.PartDestruction(other.gameObject.name);
            Destroy(gameObject);
        }       
    }
}
