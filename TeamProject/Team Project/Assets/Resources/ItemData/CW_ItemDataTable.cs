using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemDataTableData
{
    [SerializeField] private EItemType m_ItemType = EItemType.None;
    public EItemType ItemType => m_ItemType;

    [SerializeField] public int Cnt = 0;

    [SerializeField] private string m_ItemName = string.Empty;
    public string ItemName => m_ItemName;
}

[CreateAssetMenu(fileName = "ItemDataTable", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class CW_ItemDataTable : ScriptableObject
{
    [SerializeField] private ItemDataTableData[] m_ItemData;

    public List<ItemDataTableData> ItemDataList => m_ItemData.ToList();
}

