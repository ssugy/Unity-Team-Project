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

        // 인벤토리를 열었을 때, 골드 소지량과 인벤토리 아이콘을 갱신해 줌.
        UpdateGold();
        RedrawSlotUI();

        // 이후, OnChangeItem 이벤트를 구독합니다.
        inventory.onChangeItem += RedrawSlotUI;
    }
    private void OnDisable()
    {
        instance = null;
        // 이벤트를 해제합니다. 인벤토리가 다시 켜질 때 구독됩니다.
        inventory.onChangeItem -= RedrawSlotUI;
    }

    // 개별 슬롯들과 장비 아이콘들을 그림.
    private void RedrawSlotUI()
    {
        // 인벤토리 UI를 갱신하는 메소드에서 해당 함수 호출이 필요한 지 재고 필요.
        JY_CharacterListManager.s_instance.playerList[0].playerStat.CopyToTemp();

        Array.ForEach(slots, e => e.RemoveSlot());               
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];            
            slots[i].isEmpty = false;            
            slots[i].UpdateSlotUI();
        }

        // 장비 아이콘을 그림.
        // EquipPart의 0번(무기)부터 4번(다리 방어구)까지.
        for(int i = 0; i < 5; i++)
        {
            #region playrStat.equped에서 해당 부위에 장착한 아이템이 존재할 때.
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
            #region playrStat.equped에서 해당 부위에 장착한 아이템이 존재하지 않을 때.
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

    // 인벤토리 UI를 열었을 때, 골드양이 변화할 때(골드를 습득하거나 사용할 때) 실행됨.
    // RedrawSlotUI에서 실행할 필요는 없음.
    public void UpdateGold()
    {
        gold.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
    }            

    // 해당 메소드들이 실행되면 RedrawSlotUI가 실행되므로 UpdatSlotUI는 필요없음.       
    public void DestroyItem(Item _item) => inventory.RemoveItem(_item);    
}
