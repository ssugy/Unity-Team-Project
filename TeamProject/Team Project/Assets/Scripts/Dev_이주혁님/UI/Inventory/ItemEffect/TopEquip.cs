using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/TopEquip")]
public class TopEquip : ItemEffect
{
    public int def;
    public int num;
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();

        if (player != null)
        {
            if (player.playerStat.equiped.TryGetValue(EquipPart.CHEST, out Item _tmp))
            {
                _tmp.effects[1].ExecuteRole(_tmp);
                player.playerStat.equiped.Add(EquipPart.CHEST, _item);
            }
            else
            {
                player.playerStat.equiped.Add(EquipPart.CHEST, _item);
            }
            _item.equipedState = EquipState.EQUIPED;
            player.playerStat.customized[1] = num;
            player.AvatarSet();
            player.playerStat.defPoint += def;
            player.SetState();
        }        
                               
        
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.chestIcon.sprite = _item.image;
            InventoryUI.instance.chestIcon.gameObject.SetActive(true);
        }
    }
}
