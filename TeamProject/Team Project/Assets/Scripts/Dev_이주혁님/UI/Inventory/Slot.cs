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
            SetText(text, "���� ��");
        }
        else
        {
            if (item.type == ItemType.EQUIPMENT || item.itemCount <= 1)
            {
                // �������� �������̰ų�
                // ������ �ߺ� ������ 1�� �Ǵ� �� �����̹Ƿ� �۾��� �ʿ� ����
                equiped.SetActive(false);
            }
            else
            {
                // ������ �ߺ� ������ 2�� �̻��̹Ƿ� ������ ǥ����
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
