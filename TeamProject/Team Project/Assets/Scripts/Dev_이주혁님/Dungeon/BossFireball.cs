using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : Fireball
{
    Vector3 targetVec;
    Vector3 moveVec;
    private void Update()
    {
        targetVec = JY_CharacterListManager.s_instance.playerList[0].transform.position;
        transform.position = Vector3.MoveTowards(transform.position,targetVec,Time.deltaTime*10f);
    }
    private void OnEnable()
    {
        Invoke("DestroySelf", 4f);
    }
}
