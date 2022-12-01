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
            GameObject shieldSrc = Resources.Load<GameObject>("Item/Shield/" + _item.image.name);
            GameObject shield = Instantiate<GameObject>(shieldSrc);//, player.lWeaponDummy);
            Shield shieldComp = shield.GetComponent<Shield>();
            if (shieldComp.level <= player.playerStat.level)
            {
                DestroyImmediate(shieldComp.gameObject);
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
                shieldSrc = Resources.Load<GameObject>("Item/Shield/" + _item.image.name);
                shield = Instantiate<GameObject>(shieldSrc, player.lWeaponDummy);
                shieldComp = shield.GetComponent<Shield>();
                if (shieldComp != null)
                {
                    shieldComp.ApplyOptions(_item);
                    shield.name = string.Copy(shieldSrc.name);
                    shieldComp.transform.parent = player.lWeaponDummy;
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
            else
            {
                DestroyImmediate(shieldComp.gameObject);
            }

        }                                      
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeShield;
    }
}
