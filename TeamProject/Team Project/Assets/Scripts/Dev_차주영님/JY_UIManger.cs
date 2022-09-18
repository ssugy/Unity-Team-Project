using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_UIManger : MonoBehaviour
{

    public Transform profileGroup;
    public Transform menuGroup;
    bool profileSwitch;
    bool menuSwitch;
    private void Awake()
    {
        profileSwitch = false;
        menuSwitch = false;
    }
    // Start is called before the first frame update
    public void switchProfile()
    {
        if(profileSwitch == false)
        {
            profileGroup.gameObject.SetActive(true);
            profileSwitch = true;
        }
        else
        {
            profileGroup.gameObject.SetActive(false);
            menuGroup.gameObject.SetActive(false);
            profileSwitch = false;
            menuSwitch = false;
        }
    }
    public void switchMenu()
    {
        if (menuSwitch == false)
        {
            menuGroup.gameObject.SetActive(true);
            menuSwitch = true;
        }
        else
        {
            menuGroup.gameObject.SetActive(false);
            menuSwitch = false;
        }
    }
}
