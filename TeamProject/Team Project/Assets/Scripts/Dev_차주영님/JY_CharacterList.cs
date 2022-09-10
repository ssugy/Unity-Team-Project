using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_CharacterList : MonoBehaviour
{
    public GameObject infoGroup;
    public GameObject createButton;
    public bool check;
    private void Awake()
    {
        check = false;
        panelSwitch(check);
    }
    // Update is called once per frame
    void Update()
    {
        panelSwitch(check);
    }

    void panelSwitch(bool nullCheck)
    {
        if (nullCheck)
        {
            infoGroup.SetActive(false);
            createButton.SetActive(true);
        }
        else
        {
            createButton.SetActive(false);
            infoGroup.SetActive(true);
        }
    }
}
