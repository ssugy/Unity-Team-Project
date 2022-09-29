using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int atkPoint;
    public float atkMag;                // �Ϲݰ����� ����� ��, Ȥ�� ��ų�� ����� �� �̸� ���ص� ������ ���⿡ ���Ե�.
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
                Debug.Log("����");
                //enemy.enemyStat.curHP -= playerState.AttackDamage(atkMag, enemy.enemyStat.defMag);
                //Debug.Log(playerState.AttackDamage(atkMag, enemy.enemyStat.defMag));
            }
        }               
    }
}
