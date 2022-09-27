using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CartoonHeroes.SetCharacter;

public class CW_ItemManager : MonoBehaviour
{

    private static CW_ItemManager _instance;

    public static CW_ItemManager Instance => _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        else if (_instance != null && _instance != this)
        {
            Destroy(_instance);
            _instance = this;
        }

        CW_ItemDataTable curItems = Resources.Load<CW_ItemDataTable>("ItemData/ItemDataTable");

        var ItemList = curItems.ItemDataList;

        foreach (var itemDataTableData in ItemList)
        {
            if (m_ItemList.TryGetValue(itemDataTableData.ItemType, out var value) == false)
            {
                value = new List<ItemDataTableData> { itemDataTableData };
                m_ItemList.Add(itemDataTableData.ItemType, value);
            }
            else
            {
                m_ItemList[itemDataTableData.ItemType].Add(itemDataTableData);
            }
        }
    }

    private Dictionary<EItemType, List<ItemDataTableData>> m_ItemList = new Dictionary<EItemType, List<ItemDataTableData>>();
    public Dictionary<EItemType, List<ItemDataTableData>> ItemList => m_ItemList;
}
