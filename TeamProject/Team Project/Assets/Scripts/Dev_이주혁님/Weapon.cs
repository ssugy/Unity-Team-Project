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
    }
    private void Start()
    {
        player = Player.instance;
        if (player != null)
        {
            player.SetState();
            player.playerAni.SetFloat("AtkSpeed", atkSpeed);
        }
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.SetState();
            player.playerAni.SetFloat("AtkSpeed", 1f);
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Enemy"))
        {
            player.Attack(other);            
        }               
    }
}
