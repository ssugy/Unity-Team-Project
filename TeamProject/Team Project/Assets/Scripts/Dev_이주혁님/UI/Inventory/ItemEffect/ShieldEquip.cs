using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/ShieldEquip")]
public class ShieldEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        Player player = JY_CharacterListManager.s_instance.playerList[0];

        if (player != null)
        {
            if (player.playerStat.equiped.TryGetValue(EquipPart.SHIELD, out Item _tmp))
            {
                _tmp.effects[1].ExecuteRole(_tmp);
                player.playerStat.equiped.Add(EquipPart.SHIELD, _item);
            }
            else
            {
                player.playerStat.equiped.Add(EquipPart.SHIELD, _item);
            }
            _item.equipedState = EquipState.EQUIPED;
            // �̹� ������ �������� �ִ� ��� _tmp.effects[1].ExecuteRole(_tmp);���� ���� ������ �������� �ı��ϱ� ������, ����/���� �ν��Ͻ� ������ �� ������ ���־�� �Ѵ�.
            GameObject shieldSrc = Resources.Load<GameObject>("Item/Shield/" + _item.image.name);
            Instantiate<GameObject>(shieldSrc, player.lWeaponDummy);
        }                

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
        {
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
        }
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.shieldIcon.sprite = _item.image;
            InventoryUI.instance.shieldIcon.gameObject.SetActive(true);
        }                       
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeShield;
    }
}
