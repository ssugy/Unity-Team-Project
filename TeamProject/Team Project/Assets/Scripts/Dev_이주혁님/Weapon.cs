using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int atkPoint;                // ���⺰ ���ݷ�.
    public float atkSpeed;
    [HideInInspector] public float atkMag;                // �Ϲݰ����� ����� ��, Ȥ�� ��ų�� ����� �� �̸� ���ص� ������ ���⿡ ���Ե�.
    [HideInInspector] public Player player;

    [HideInInspector] public static Weapon weapon;
    [HideInInspector] public static BoxCollider weapoonHitbox;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {        
        weapon = GetComponent<Weapon>();
        weapoonHitbox = GetComponentInChildren<BoxCollider>();
        player = Player.instance;
        if (player != null && player.playerAni != null)
        {
            player.playerAni.SetFloat("AtkSpeed", atkSpeed);
        }
        Player.instance.SetState();
    }    
    private void OnDisable()
    {
        weapon = null;
        weapoonHitbox = null;
        if (player != null && player.playerAni != null)
        {            
            player.playerAni.SetFloat("AtkSpeed", 1f);
        }
        Player.instance.SetState();
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Enemy"))
        {
            player.Attack(other);            
        }               
    }
}
