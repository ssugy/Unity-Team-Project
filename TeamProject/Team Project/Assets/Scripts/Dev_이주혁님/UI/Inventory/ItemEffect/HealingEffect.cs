using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Healing")]
public class HealingEffect : ItemEffect
{
    public int healingPoint;
    public override void ExecuteRole(Item _item)
    {
        Debug.Log("체력 회복");
        Player.instance.playerStat.CurHP += healingPoint;        
    }
}
