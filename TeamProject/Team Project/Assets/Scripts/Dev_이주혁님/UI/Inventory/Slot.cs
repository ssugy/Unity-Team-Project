using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{    
    public Image icon;
    public Item item;
    public bool isEmpty;
    public InfoPanel infoPanel;
    public GameObject equiped;

    private void Awake()
    {        
        item = null;
        isEmpty = true;
    }    
    
    private void SetText(Text text, string msg)
    {
        if (text != null)
        {
            text.text = msg;
        }
    }

    public void UpdateSlotUI()
    {
        icon.sprite = item.image;
        Text text = equiped.GetComponentInChildren<Text>();
        if (item.equipedState == EquipState.EQUIPED)
        {
            equiped.SetActive(true);
            SetText(text, "장착 중");
        }
        else
        {
            if (item.type == ItemType.EQUIPMENT || item.itemCount <= 1)
            {
                // 아이템이 장착형이거나
                // 아이템 중복 갯수가 1개 또는 그 이하이므로 글씨가 필요 없음
                equiped.SetActive(false);
            }
            else
            {
                // 아이템 중복 갯수가 2개 이상이므로 갯수를 표현함
                equiped.SetActive(true);
                SetText(text, item.itemCount.ToString());
            }
        }
        icon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        isEmpty = true;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        if (!isEmpty)
        {
            infoPanel.SetInformation(item);
            infoPanel.gameObject.SetActive(true);            
        }
    }
}
