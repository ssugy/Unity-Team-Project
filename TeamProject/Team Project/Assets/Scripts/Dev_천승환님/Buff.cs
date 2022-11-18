using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum Type { HP, AtkPoint, SpRecover, HPRecover, AtkSpeed, defPoint };
    public Type type;
    public int value;
   

    void Update()
    {
        //transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }
}
