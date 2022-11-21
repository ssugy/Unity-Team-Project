using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_BossExplosion : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyExplosion", 1f);
    }
    void DestroyExplosion()
    {
       Destroy(gameObject);
    }
}
