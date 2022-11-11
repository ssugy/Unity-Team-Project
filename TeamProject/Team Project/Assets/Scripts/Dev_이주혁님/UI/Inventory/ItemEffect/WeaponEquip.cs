using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/WeaponEquip")]
public class WeaponEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();

        if (player != null)
        {
            if (player.playerStat.equiped.TryGetValue(EquipPart.WEAPON, out Item _tmp))
            {
                _tmp.effects[1].ExecuteRole(_tmp);
                player.playerStat.equiped.Add(EquipPart.WEAPON, _item);
            }
            else
            {
                player.playerStat.equiped.Add(EquipPart.WEAPON, _item);
            }
            _item.equipedState = EquipState.EQUIPED;
            // �̹� ������ �������� �ִ� ��� _tmp.effects[1].ExecuteRole(_tmp);���� ���� ������ �������� �ı��ϱ� ������, ����/���� �ν��Ͻ� ������ �� ������ ���־�� �Ѵ�.
            GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
            Instantiate<GameObject>(weaponSrc, player.rWeaponDummy);
        }              

        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }   
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.weaponIcon.sprite = _item.image;
            InventoryUI.instance.weaponIcon.gameObject.SetActive(true);
        }          
    }
}
