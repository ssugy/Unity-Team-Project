using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public static InventoryUI instance;

    public Image weaponIcon;
    public Image shieldIcon;
    public Image helmetIcon;
    public Image chestIcon;
    public Image legIcon;
    public Text gold;
    public GameObject inventoryPanel;        
    public Slot[] slots;
    public Transform slotHolder;
    private void Awake()
    {
        instance = this;
        inventory = JY_CharacterListManager.s_instance.invenList[0];
        slots = slotHolder.GetComponentsInChildren<Slot>();
    }
    private void OnEnable()
    {
        instance ??= this;

        // �κ��丮�� ������ ��, ��� �������� �κ��丮 �������� ������ ��.
        UpdateGold();
        RedrawSlotUI();

        // ����, OnChangeItem �̺�Ʈ�� �����մϴ�.
        inventory.onChangeItem += RedrawSlotUI;
    }
    private void OnDisable()
    {
        instance = null;
        // �̺�Ʈ�� �����մϴ�. �κ��丮�� �ٽ� ���� �� �����˴ϴ�.
        inventory.onChangeItem -= RedrawSlotUI;
    }

    // ���� ���Ե�� ��� �����ܵ��� �׸�.
    private void RedrawSlotUI()
    {
        // �κ��丮 UI�� �����ϴ� �޼ҵ忡�� �ش� �Լ� ȣ���� �ʿ��� �� ��� �ʿ�.
        JY_CharacterListManager.s_instance.playerList[0].playerStat.CopyToTemp();

        Array.ForEach(slots, e => e.RemoveSlot());               
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];            
            slots[i].isEmpty = false;            
            slots[i].UpdateSlotUI();
        }

        // ��� �������� �׸�.
        // EquipPart�� 0��(����)���� 4��(�ٸ� ��)����.
        for(int i = 0; i < 5; i++)
        {
            #region playrStat.equped���� �ش� ������ ������ �������� ������ ��.
            if (JY_CharacterListManager.s_instance.playerList[0]
                .playerStat.equiped.TryGetValue((EquipPart)i,out Item tmp))
            {
                switch (i)
                {
                    case 0:
                        {
                            weaponIcon.sprite = tmp.image;
                            weaponIcon.gameObject.SetActive(true);
                            break;
                        }
                    case 1:
                        {
                            shieldIcon.sprite = tmp.image;
                            shieldIcon.gameObject.SetActive(true);
                            break;
                        }
                        /*
                    case 2:
                        {
                            helmetIcon.sprite = tmp.image;
                            helmetIcon.gameObject.SetActive(true);
                            break;
                        }
                        */
                    case 3:
                        {
                            chestIcon.sprite = tmp.image;
                            chestIcon.gameObject.SetActive(true);
                            break;
                        }
                    case 4:
                        {
                            legIcon.sprite = tmp.image;
                            legIcon.gameObject.SetActive(true);
                            break;
                        }
                }
            }
            #endregion
            #region playrStat.equped���� �ش� ������ ������ �������� �������� ���� ��.
            else
            {
                switch (i)
                {
                    case 0:
                        {
                            weaponIcon.sprite = null;
                            weaponIcon.gameObject.SetActive(false);
                            break;
                        }
                    case 1:
                        {
                            shieldIcon.sprite = null;
                            shieldIcon.gameObject.SetActive(false);
                            break;
                        }
                        /*
                    case 2:
                        {
                            helmetIcon.sprite = null;
                            helmetIcon.gameObject.SetActive(false);
                            break;
                        }
                        */
                    case 3:
                        {
                            chestIcon.sprite = null;
                            chestIcon.gameObject.SetActive(false);
                            break;
                        }
                    case 4:
                        {
                            legIcon.sprite = null;
                            legIcon.gameObject.SetActive(false);
                            break;
                        }
                }
            }
            #endregion
        }

    } 

    // �κ��丮 UI�� ������ ��, ������ ��ȭ�� ��(��带 �����ϰų� ����� ��) �����.
    // RedrawSlotUI���� ������ �ʿ�� ����.
    public void UpdateGold()
    {
        gold.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
    }            

    // �ش� �޼ҵ���� ����Ǹ� RedrawSlotUI�� ����ǹǷ� UpdatSlotUI�� �ʿ����.       
    public void DestroyItem(Item _item) => inventory.RemoveItem(_item);    
}
