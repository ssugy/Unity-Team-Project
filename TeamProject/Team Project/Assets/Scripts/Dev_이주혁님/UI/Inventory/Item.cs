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
    public ItemType type;
    public EquipState equipedState;
    public string name;
    [TextArea(3,5)]
    public string explanation;
    public Sprite image;
    public List<ItemEffect> effects;    
   
    public void Use()
    {        
        foreach(ItemEffect one in effects)
        {
            one.ExecuteRole();
        }             
    }
    public void Equip()
    {        
        switch (equipedState)
        {
            case EquipState.EQUIPED:
                equipedState = EquipState.UNEQUIPED;                
                foreach (ItemEffect one in effects)
                {
                    one.ExecuteRole();
                }
                break;
            case EquipState.UNEQUIPED:
                equipedState = EquipState.UNEQUIPED;                
                foreach (ItemEffect one in effects)
                {
                    one.ExecuteRole();
                }
                break;
        }        
    }
}

