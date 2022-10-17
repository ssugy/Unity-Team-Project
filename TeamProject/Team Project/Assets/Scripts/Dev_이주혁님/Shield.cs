using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float defPro;                    // ���� ��� �� ����� �����.    
    [HideInInspector] public Player player; 
    [HideInInspector] public static Shield shield;
    private void OnEnable()
    {
        shield = GetComponent<Shield>();        
        player = Player.instance;        
        Player.instance.SetState();
    }
    private void OnDisable()
    {
        shield = null;               
        Player.instance.SetState();
    }
}
