using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public float time;  // 팝업창이 유지되는 시간.
    private void OnEnable()
    {
        CancelInvoke("Inactive");
        Invoke("Inactive", time);
    }
    private void Inactive()
    {
        gameObject.SetActive(false);
    }
}
