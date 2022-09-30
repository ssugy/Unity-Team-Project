using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_PlayerReturn : MonoBehaviour
{
    public static JY_PlayerReturn instance;
    public GameObject playerOrigin;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject getPlayerOrigin()
    {
        return playerOrigin;
    }
}
