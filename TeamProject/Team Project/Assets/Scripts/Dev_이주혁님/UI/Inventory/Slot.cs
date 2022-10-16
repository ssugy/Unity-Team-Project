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

    private void Awake()
    {        
        item = null;
        isEmpty = true;
    }    
    
    public void UpdateSlotUI()
    {
        icon.sprite = item.image;
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
