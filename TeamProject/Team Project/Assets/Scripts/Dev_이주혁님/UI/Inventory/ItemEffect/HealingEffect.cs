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
        // ȸ���� ���� �ɼ��� ������ ���� ���
        float total = (float)JY_CharacterListManager.s_instance.playerList[0].playerStat.addedRecover / 100.0f * healingPoint;
        JY_CharacterListManager.s_instance.playerList[0].playerStat.CurHP += (int)total;        
    }
}
