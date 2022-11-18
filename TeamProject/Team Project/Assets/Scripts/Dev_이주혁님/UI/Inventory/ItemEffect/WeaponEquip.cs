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
            // 이미 장착한 아이템이 있는 경우 _tmp.effects[1].ExecuteRole(_tmp);에서 현재 장착한 아이템을 파괴하기 때문에, 무기/방패 인스턴스 생성은 그 다음에 해주어야 한다.
            GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
            GameObject weapon = Instantiate(weaponSrc, player.rWeaponDummy);
            weapon.name = string.Copy(weaponSrc.name);
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
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeWeapon;
    }
}
