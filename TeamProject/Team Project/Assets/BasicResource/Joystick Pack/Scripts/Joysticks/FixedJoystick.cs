using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public static FixedJoystick instance;

    private void Awake()
    {
        instance = this;
    }


}