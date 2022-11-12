using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    public float atkDmg;                    // 마법 공격 사용 시 적용될 공격력.    
    [HideInInspector] public Player player;
    [HideInInspector] public static Staff staff;
    private void OnEnable()
    {
        staff = GetComponent<Staff>();
        player = Player.instance;
        player?.SetState();
    }
    private void OnDisable()
    {
        staff = null;
        player?.SetState();
    }
}
