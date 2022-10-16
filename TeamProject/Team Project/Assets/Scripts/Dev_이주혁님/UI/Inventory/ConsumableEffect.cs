using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemEft/Consimable")]
public class ConsumableEffect : ItemEffect
{
    public int healingPoint;
    public override bool ExecuteRole()
    {
        Debug.Log("체력 회복");
        Player.instance.playerStat.CurHP += healingPoint;
        return true;
    }
}
