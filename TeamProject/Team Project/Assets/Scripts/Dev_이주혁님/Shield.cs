using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float defPro;                    // 막기 사용 시 적용될 방어율.    
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
