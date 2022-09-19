using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EItemType
{
    None,
    Equip,
    Consumable,
    Etc,
}

public class ItemData 
{
    public int id = 0;
    public EItemType itemType = EItemType.None;
    public string ItemName = string.Empty;
    public string itemAbilityName = string.Empty;
    public float itemAbilityValue = 0f;
    public string itemNeedJob = string.Empty;
    public int itemNeedLV = 0;

    public int cnt = 0;
}


public class ItemDataManager
{
    public static ItemDataManager Instance = null;
    private Dictionary<EItemType, List<ItemData>> ItemDic = new Dictionary<EItemType, List<ItemData>>();

    public bool TryGetItemDataListInType(EItemType type, out List<ItemData> itemDataList)
    {
        ItemDic.TryGetValue(type, out itemDataList);

        return itemDataList != null;
    }

    public void GetItem(ItemData data)
    {
        var type = data.itemType;

        if (ItemDic.TryGetValue(type, out var result))
        {
            var curItemVolume = result.Count;
            if (result.Count + 1 >= CW_Define.INVENTORY_LIMIT)
                return;
        }

        else
        {

        }
    }



    public void RemoveItem(ItemData data, int removeCnt = 1)
    {
        if (ItemDic.TryGetValue(data.itemType, out var list) == false)
        {
            Debug.LogError("Error");
            return;
        }

        for (var i = 0; i < list.Count; i++)
        {
            var curItem = list[i];
            if (curItem == null)
            {
                Debug.LogError("Error");
                continue;
            }

            if (curItem.ItemName.Equals(data.ItemName))
            {
                curItem.cnt -= removeCnt;
                if (curItem.cnt <= 0)
                {
                    list.Remove(curItem);
                }

                list[i] = curItem;

                break;
            }
        }
    }

}



public class CW_UIInventory : MonoBehaviour
{
    [Header(" # Btn Exit")]
    [SerializeField] private Button ExitBtn = null;

    [Header(" # Btn Main")]
    [SerializeField] private Button EquipBtn = null;
    [SerializeField] private Button ShowBtn = null;

    [Header(" # Btn Item List")]
    [SerializeField] private Button EquipTypeListBtn = null;
    [SerializeField] private Button ConsumableTypeListBtn = null;
    [SerializeField] private Button EtcTypeListBtn = null;

    [Header(" # Inventory Item List")]
    [SerializeField] private CW_UIInventoryItemList ItemList = null;

    [Header(" # Inventory Popup")]
    [SerializeField] private CW_UIInventoryEquipPopup EquipPopup = null;

    public EItemType InventoryItemListType { get; private set; } = EItemType.None;
    private List<ItemData> CurTypeItemList = null;

    private void Awake()
    {
        CurTypeItemList = new List<ItemData>();
        InventoryItemListType = EItemType.Equip;

            CW_CommonFunc.SetRemoveAndListener(EquipBtn, OnClickExit);

            CW_CommonFunc.SetRemoveAndListener(EquipTypeListBtn, callback:() => OnClickSetItemList(EItemType.Equip));
            CW_CommonFunc.SetRemoveAndListener(ConsumableTypeListBtn, callback:() => OnClickSetItemList(EItemType.Consumable));
            CW_CommonFunc.SetRemoveAndListener(EtcTypeListBtn, callback:()=> OnClickSetItemList(EItemType.Etc));
    }

    private void ShowPopup(ItemData data)
    {
        if (data == null)
            return;

        EquipPopup.SetData(data, OnClickEquipBtn);
    }

    public void ShowInventory()
    {
        SetItemList(InventoryItemListType);
        this.gameObject.SetActive(true);
    }

    private void OnClickEquipBtn(ItemData data)
    {

    }

    private void SetItemList(EItemType itemType)
    {
        if (itemType == EItemType.None)
            return;

        if (ItemDataManager.Instance.TryGetItemDataListInType(itemType, out CurTypeItemList) == false)
            Debug.LogError(message:$"Can't Find {itemType.ToString()}");
            return;

        ItemList.SetData(CurTypeItemList, OnClickEquipBtn);
    }
    #region Button Callback
    public void OnClickShowInventory()
    {
        this.gameObject.SetActive(true);
    }

    public void OnClickExit()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickSetItemList(EItemType itemType)
    {
        if (this.InventoryItemListType == itemType)
            return;

        if (itemType == EItemType.None)
            return;


    }



    #endregion
}
