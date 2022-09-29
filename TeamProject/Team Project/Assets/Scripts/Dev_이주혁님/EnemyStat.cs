using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyStat
{
    public int HP;
    public int curHP;
    public int atkPoint;
    public float atkMag;
    public float defMag;
    public List<ItemData> dropItem;
    public int dropGold;
    public int dropExp;
}
