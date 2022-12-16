using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Workshop : MonoBehaviour
{          
    public Image icon;
    public Text nameText;
    public ItemMethod method;

    public Item item;
    private Button mine;

    private void OnEnable()
    {
        mine = GetComponent<Button>();
        mine.onClick.AddListener(() => OnClick());
    }

    // ���� Ŭ�� �� ����� �޼ҵ�.
    public void OnClick()
    {
        if (Workshop.workshop != null)
        {
            Workshop.workshop.selectedMethod = method;
            Workshop.workshop.selectedItem = item;
            if (item != null)
            {
                
            }
            Workshop.workshop.UpdatePanel();
        }               
    }

}
