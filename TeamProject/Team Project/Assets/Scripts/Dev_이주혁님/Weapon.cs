using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int atkPoint;                // 무기별 공격력.
    public float atkMag;                // 일반공격을 사용할 때, 혹은 스킬을 사용할 때 미리 정해둔 배율이 여기에 대입됨.
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
