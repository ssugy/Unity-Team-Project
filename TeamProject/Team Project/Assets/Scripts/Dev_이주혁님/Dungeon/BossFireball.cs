using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : Fireball
{
    Vector3 targetVec;
    Vector3 moveVec;
    private void Update()
    {
        transform.Translate(moveVec * Time.deltaTime);
    }
    private void OnEnable()
    {
        targetVec = JY_CharacterListManager.s_instance.playerList[0].transform.position;
        moveVec = targetVec - transform.position;
        Invoke("DestroySelf", 4f);
    }
}
