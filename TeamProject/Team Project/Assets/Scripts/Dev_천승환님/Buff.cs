using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum Type { HP, AtkPoint, SpRecover, HPRecover, AtkSpeed, defPoint };
    public Type type;
    public int value;
}
