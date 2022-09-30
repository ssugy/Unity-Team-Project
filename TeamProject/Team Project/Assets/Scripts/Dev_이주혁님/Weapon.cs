using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int atkPoint;                // ���⺰ ���ݷ�.
    public float atkMag;                // �Ϲݰ����� ����� ��, Ȥ�� ��ų�� ����� �� �̸� ���ص� ������ ���⿡ ���Ե�.
    public Player player;

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
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.playerStat.atkPoint -= atkPoint;
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
