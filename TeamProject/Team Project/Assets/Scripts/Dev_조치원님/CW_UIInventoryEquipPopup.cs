using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CW_UIInventoryEquipPopup : MonoBehaviour
{
    [Header("# Text Item Name")]
    [SerializeField] private Text ItemName = null;

    [Header("# Text Item Ability")]
    [SerializeField] private Text ItemAddAbilityName = null;
    [SerializeField] private Text ItemAddAbilityValue = null;

    [Header("# Text Item Condition")]
    [SerializeField] private Text ItemNeedLV = null;
    [SerializeField] private Text ItemNeedJob = null;

    [Header("# Text Item Item")]
    [SerializeField] private Button EquipBtn = null;
    [SerializeField] private Button RemoveBtn = null;
    [SerializeField] private Button DetailInfoBtn = null;
    [SerializeField] private Button ExitBtn = null;

    [Header("# Image Item")]
    [SerializeField] private Image ItemImage = null;

    private Action<ItemData> EquipCallback = null;
    private ItemData CurItemData = null;

    private void Awake()
    {
        CW_CommonFunc.SetRemoveAndListener(EquipBtn, OnClickEquipBtn);
        CW_CommonFunc.SetRemoveAndListener(RemoveBtn, OnClickRemove);
        CW_CommonFunc.SetRemoveAndListener(DetailInfoBtn, OnClickDetailInfo);
        CW_CommonFunc.SetRemoveAndListener(ExitBtn, OnClickExit);
    }

    public void SetData(ItemData data, Action<ItemData> callback)
    {
        ResetUI();

        this.CurItemData = data;
        this.EquipCallback = callback;

        SetUI();
        gameObject.SetActive(true);
    }

    private void OnClickEquipBtn()
    {
        if (CurItemData == null)
            return;

        EquipCallback?.Invoke(CurItemData);
    }

    private void OnClickRemove()
    {
        if (CurItemData == null)
            return;

        ItemDataManager.Instance.RemoveItem(CurItemData, 1);
        this.gameObject.SetActive(false);
        ResetUI();
        
    }

    private void OnClickDetailInfo()
    {

    }

    private void OnClickExit()
    {
        this.gameObject.SetActive(false);
        ResetUI();
    }

    private void SetUI()
    {
        if(ResourceManager.Instance.GetItemSprite(CurItemData.itemType, CurItemData.ItemName, out var spr))
        {
            ItemImage.sprite = spr;
        }
        else
        {
            return;
        }

        ItemName.text = CurItemData.ItemName;
        ItemAddAbilityName.text = CurItemData.itemAbilityName;
        ItemAddAbilityValue.text = CurItemData.itemAbilityValue.ToString();
        ItemNeedLV.text = CurItemData.itemNeedLV.ToString();
        ItemNeedJob.text = CurItemData.itemNeedJob;
        CurItemData = null;
    }

    private void ResetUI()
    {
        ItemImage.sprite = null;
        ItemName.text = string.Empty;
        ItemAddAbilityName.text = string.Empty;
        ItemAddAbilityValue.text = string.Empty;
        ItemNeedLV.text = string.Empty;
        ItemNeedJob.text = string.Empty;
        CurItemData = null;
    }
}