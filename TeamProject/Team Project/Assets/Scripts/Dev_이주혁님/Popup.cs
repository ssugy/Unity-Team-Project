using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public float time;  // �˾�â�� �����Ǵ� �ð�.
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
