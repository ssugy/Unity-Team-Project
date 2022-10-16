using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Healing")]
public class HealingEffect : ItemEffect
{
    public int healingPoint;
    public override void ExecuteRole()
    {
        Debug.Log("ü�� ȸ��");
        Player.instance.playerStat.CurHP += healingPoint;        
    }
}
