using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Healing")]
public class HealingEffect : ItemEffect
{
    public int healingPoint;
    public override void ExecuteRole(Item _item)
    {
        Debug.Log("ü�� ȸ��");
        JY_CharacterListManager.s_instance.playerList[0].playerStat.CurHP += healingPoint;        
    }
}
