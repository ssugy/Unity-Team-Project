using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipOption;

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
    // 인벤토리를 세이브 파일에 저장할 때 저장하는 항목들. (나머지 데이터는 DB에서 로드함)
    public Item(ItemType _type = ItemType.NONE, EquipState _state = EquipState.NONE, 
        string _name = null, int _count = 0, EquipOption _option = null)
    {
        type = _type;
        equipedState = _state;
        name = _name;        
        itemCount = _count;        
        option = _option;
    }
    
    // 세이브 파일에 저장되는 항목들.
    public ItemType type = ItemType.NONE;
    public EquipState equipedState = EquipState.NONE;
    public string name = "null";    
    public int itemCount;
    public EquipOption option = null;

    // 세이브 파일에 저장되지 않는 항목들.
    public int level;
    public int price;
    [TextArea(3, 5)]
    public string explanation = "null";
    public Sprite image = null;         // 아이템 아이콘.
    public List<ItemEffect> effects = null;
       

    public void Use() => effects.ForEach(e => e.ExecuteRole(this));    
    public void Equip() => effects[0].ExecuteRole(this);    
    public void Unequip() => effects[1].ExecuteRole(this);
    
    public Item Copy()          // 아이템 DB에서 아이템 클래스를 불러올 때 Shallow Copy를 막기 위해 만든 메소드.
    {
        Item copied = new Item();
        copied.type = this.type;
        copied.equipedState = this.equipedState;
        copied.name = this.name;
        copied.itemCount = this.itemCount;
        copied.level = this.level;
        copied.price = this.price;
        copied.explanation = this.explanation;
        copied.image = this.image;
        copied.effects = this.effects;
        // 옵션 저장
        copied.option = this.option;
        return copied;
    }

    // 아이템의 이름 정보만 갖고 있을 때,
    // 이름으로부터 아이템의 레벨 제한, 가격, 설명, 이미지, 이펙트를 불러옴.
    public void ShallowCopy()
    {
        Item tmp = new Item();        
        for (int i=0;i< ItemDatabase.s_instance.itemDB.Count; i++)
        {
            if(this.name.Equals(ItemDatabase.s_instance.itemDB[i].name))
            {
                tmp = ItemDatabase.s_instance.itemDB[i];
                break;
            }
        }
        this.level = tmp.level;
        this.price = tmp.price;
        this.explanation = tmp.explanation;
        this.image = tmp.image;
        this.effects = tmp.effects;
    }
    public void SetOption()
    {
        if (type == ItemType.EQUIPMENT)
        {
            EquipType itemType = (EquipType)effects[0].GetType();
            option = new EquipOption(itemType);
        }
    }
}

