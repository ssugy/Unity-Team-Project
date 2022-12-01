using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/WeaponEquip")]
public class WeaponEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        Player player = JY_CharacterListManager.s_instance.playerList[0];

        if (player != null)
        {
            // �̹� ������ �������� �ִ� ��� _tmp.effects[1].ExecuteRole(_tmp);���� ���� ������ �������� �ı��ϱ� ������, ����/���� �ν��Ͻ� ������ �� ������ ���־�� �Ѵ�.
            GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
            GameObject weapon = Instantiate(weaponSrc);//, player.rWeaponDummy);
            Weapon weaponComp = weapon.GetComponent<Weapon>();
            if (weaponComp.level <= player.playerStat.level) 
            {  
                DestroyImmediate(weaponComp.gameObject);
                //weaponComp.gameObject.SetActive(false);

                // ������ ������ �� �������� ���Ⱑ ������.
                if (player.playerStat.equiped.TryGetValue(EquipPart.WEAPON, out Item _tmp))
                {
                    // �̹� ���⸦ ���� ��� ������ �� ����
                    _tmp.effects[1].ExecuteRole(_tmp);
                    player.playerStat.equiped.Add(EquipPart.WEAPON, _item);
                }
                else
                {
                    // ���Ⱑ ��� �ٷ� ����
                    player.playerStat.equiped.Add(EquipPart.WEAPON, _item);
                }
                _item.equipedState = EquipState.EQUIPED;
                weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
                weapon = Instantiate(weaponSrc, player.rWeaponDummy);
                weaponComp = weapon.GetComponent<Weapon>();
                if (weaponComp != null)
                {
                    weaponComp.ApplyOptions(_item);
                    weapon.name = string.Copy(weaponSrc.name);
                    weaponComp.transform.parent = player.rWeaponDummy;
                }

                if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
                {
                    JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
                }
                if (InventoryUI.instance != null)
                {
                    InventoryUI.instance.weaponIcon.sprite = _item.image;
                    InventoryUI.instance.weaponIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                DestroyImmediate(weaponComp.gameObject);
            }
        }                     
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeWeapon;
    }
}
