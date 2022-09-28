using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    private void OnEnable()
    {
        CancelInvoke("Inactive");
        Invoke("Inactive", 1.5f);
    }
    private void Inactive()
    {
        gameObject.SetActive(false);
    }
}
