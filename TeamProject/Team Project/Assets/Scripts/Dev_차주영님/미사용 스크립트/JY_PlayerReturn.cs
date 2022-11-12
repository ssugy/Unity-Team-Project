using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_PlayerReturn : MonoBehaviour
{
    public static JY_PlayerReturn instance;
    
    void Awake() => instance ??= this;
    private void OnDisable()
    {
        instance = null;
    }

    public Transform GetPlayerOrigin() => transform;    
}
