using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CW_UIInventoryItemList : MonoBehaviour
{
    public delegate void ItemClickDelegate(ItemData data);

    public List<CW_UIItem> ItemList = null;
    public void SetData(List<ItemData> curItemData, Action<ItemData> dat )
    {
        int curItemCnt = curItemData.Count;

        if(curItemCnt >= ItemList.Count)
        {
            Debug.LogError("Error");
            return;
        }

        ResetData();

        SetAllData(curItemData, curItemCnt, dat);

    }

    public void ResetData()
    {
        CW_CommonFunc.SafetyFor(ItemList, cur =>
        {
            if (cur.gameObject.activeSelf == true)
                return;

            cur.Reset();
        });
    }

    private void SetAllData(List<ItemData> curItemData, int len, Action<ItemData> dat)
    {
        for (var i = 0; i < ItemList.Count; i++)
        {
            var slot = ItemList[i];
            if (i < len)
            {
                var ItemData = curItemData[i];

                slot.SetData(ItemData, dat);
            }
        }
    }
}



public class ResourceManager
{
    public static ResourceManager Instance = null;

    public bool GetItemSprite(EItemType type, string itemName, out Sprite spr)
    {
        string path = $@"Sprite/Item/{type.ToString()}/{itemName}";
        spr = Resources.Load<Sprite>(path);
        return spr != null;
    }

 /*   public void T(EItemType type, string itemName, out Sprite spr)
    {
        string path = $@"Sprite/Item/{type.ToString()}/{itemName}";
        SpriteAtlas tmp = Resources.Load<SpriteAtlas>(path);
        spr = tmp.GetSprite(itemName);
        return spr != null;
    }*/
}
