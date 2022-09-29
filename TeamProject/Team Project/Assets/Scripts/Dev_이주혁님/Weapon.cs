using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int atkPoint;
    public float atkMag;                // 일반공격을 사용할 때, 혹은 스킬을 사용할 때 미리 정해둔 배율이 여기에 대입됨.
    public PlayerState playerState;    
    
    void Start()
    {
        playerState = PlayerController.player.GetComponent<PlayerState>();
        playerState.playerStat.atkPoint += atkPoint;
    }
    private void OnDisable()
    {
        playerState.playerStat.atkPoint -= atkPoint;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("공격");
                //enemy.enemyStat.curHP -= playerState.AttackDamage(atkMag, enemy.enemyStat.defMag);
                //Debug.Log(playerState.AttackDamage(atkMag, enemy.enemyStat.defMag));
            }
        }               
    }
}
