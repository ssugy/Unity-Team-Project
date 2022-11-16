using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int atkPoint;                // ���⺰ ���ݷ�.
    public float atkSpeed;
    public float atkMag;                // �Ϲݰ����� ����� ��, Ȥ�� ��ų�� ����� �� �̸� ���ص� ������ ���⿡ ���Ե�.
    public Player player;    
    public BoxCollider weaponHitbox;

    private void OnEnable()
    {                      
        player = GetComponentInParent<Player>();                               
        weaponHitbox = GetComponentInChildren<BoxCollider>();
        if (player != null && player.playerAni != null)
            player.playerAni.SetFloat("AtkSpeed", atkSpeed);
        if (player != null)
        {
            player.rWeapon = this;
            player.SetState();
        }            
    }    
    private void OnDisable()
    {
        
        weaponHitbox = null;
        if (player != null && player.playerAni != null)
            player.playerAni.SetFloat("AtkSpeed", 1f);
        if (player != null)
        {
            player.rWeapon = null;
            player.SetState();
        }        
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Enemy"))
            player.Attack(other);                       
    }
}
