using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_QuickSlot : MonoBehaviour
{
    public Image SlotIcon;
    public Text SlotCount;
    public Item EquipItem;
    Item EmptySlot;
    Button button;
    private void Start()
    {
        EmptySlot = new Item();
        button = GetComponent<Button>();
    }
    public void EquipQuickSlot(Item _item)
    {
        SlotIcon.gameObject.SetActive(true);
        SlotIcon.sprite = _item.image;
        SlotCount.text = _item.itemCount.ToString();
        EquipItem = _item;
        button.onClick.AddListener(() => QuickSlotUse(_item));
    }
    public void QuickSlotUse(Item _item)
    {
        if (_item.itemCount > 0)
        {
            _item.Use();
            JY_CharacterListManager.s_instance.invenList[0].RemoveItem(_item);
            SlotCount.text = _item.itemCount.ToString();
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_POTION);
        }
        if (_item.itemCount <= 0)
        {
            SlotCount.text="0";
            button.onClick.RemoveAllListeners();
        }

    }
    public void ClearQuickSlot()
    {
        EquipItem = EmptySlot;
        SlotIcon.sprite = null;
        SlotIcon.gameObject.SetActive(false);
        button.onClick.RemoveAllListeners();
    }
}
