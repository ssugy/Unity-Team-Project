using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class CW_CommonFunc
{
    // Only Use One Callback
    public static void SetRemoveAndListener(Button btn, UnityAction callback)
    {
        if (btn == null)
            return;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(callback);
    }

    public static void SafetyFor<T>(IList<T> list, Action<T> callback)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var cur = list[i];
            if (cur == null)
                continue;

            callback?.Invoke(cur);
        }
    }

    public static void SafetyAction<T>(T src, Action action)
    {
        if (src == null)
            return;


        action?.Invoke();
    }
}
