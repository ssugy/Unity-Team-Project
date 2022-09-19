using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CW_UIItem : MonoBehaviour
{
    private ItemData Data = null;
    [SerializeField] private Image Image = null;
    [SerializeField] private Button button = null;
    private Action<ItemData> callback = null;

    public void Awake()
    {
        CW_CommonFunc.SetRemoveAndListener(button, () =>
         {
             callback.Invoke(Data);
         });
    }
    public void Reset()
    {
        Data = null;
        Image.sprite = null;
        callback = null;
    }

    public void SetData(ItemData data, Action<ItemData> dat)
    {
        Data = data;
        callback = dat;

        if(Data == null)
        {
            this.gameObject.SetActive(false);
                return;
        }

       if (ResourceManager.Instance.GetItemSprite(data.itemType, data.ItemName, out Sprite spr) == false)
        {
            this.gameObject.SetActive(false);
            return;
        }

        Image.sprite = spr;
    }
}
