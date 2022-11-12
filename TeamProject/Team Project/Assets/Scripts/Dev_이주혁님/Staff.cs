using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    public float atkDmg;                    // ���� ���� ��� �� ����� ���ݷ�.    
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
