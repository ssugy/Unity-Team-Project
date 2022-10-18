using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE,
    EQUIPMENT,
    CONSUMABLE,
    INGREDIENTS
}
public enum EquipState
{
    NONE,
    UNEQUIPED,
    EQUIPED    
}

[System.Serializable]
public class Item
{
    public ItemType type = ItemType.NONE;
    public EquipState equipedState = EquipState.NONE;
    public string name = "null";
    [TextArea(3, 5)]
    public string explanation = "null";
    public Sprite image = null;
    public List<ItemEffect> effects = null;
   
    public void Use()
    {        
        foreach(ItemEffect one in effects)
        {
            one.ExecuteRole(this);
        }             
    }
    public void Equip()
    {
        this.equipedState = EquipState.EQUIPED;        
        if(Player.instance.playerStat.equiped.TryGetValue(EquipPart.WEAPON, out Item _val))
        {
            effects[1].ExecuteRole(_val);
        }        
        effects[0].ExecuteRole(this);        
    }
    public void Unequip()
    {
        this.equipedState = EquipState.UNEQUIPED;
        effects[1].ExecuteRole(this);
    }
    public Item Copy()          // 아이템 DB에서 아이템 클래스를 불러올 때 Shallow Copy를 막기 위해 만든 메소드.
    {
        Item copied = new Item();
        copied.type = this.type;
        copied.equipedState = this.equipedState;
        copied.name = this.name;
        copied.explanation = this.explanation;
        copied.image = this.image;
        copied.effects = this.effects;
        return copied;
    }

    // 아이템의 이름 정보만 갖고 있을 때, 이름으로부터 아이템의 설명, 이미지, 이펙트를 불러옴.
    public void ShallowCopy()
    {
        Item tmp = new Item();
        for (int i=0;i< ItemDatabase.instance.itemDB.Count; i++)
        {
            if(this.name.Equals(ItemDatabase.instance.itemDB[i].name))
            {
                tmp = ItemDatabase.instance.itemDB[i];
                break;
            }
        }
        this.explanation = tmp.explanation;
        this.image = tmp.image;
        this.effects = tmp.effects;
    }
}

