using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float defPro;                    // ���� ��� �� ����� �����.    
    public Player player; 
    
    private void OnEnable()
    {        
        player = GetComponentInParent<Player>();
        if (player != null)
        {
            player.lWeapon = this;
            player.SetState();
        }
        
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.lWeapon = null;
            player.SetState();
        }
    }
}
