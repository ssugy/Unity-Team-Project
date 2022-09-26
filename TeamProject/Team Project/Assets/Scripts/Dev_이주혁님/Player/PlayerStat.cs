using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatn
{
    [Header("Adjustable")]
    public byte health;
    public byte stamina;
    public byte strength;
    public byte dexterity;

    [Header("Statistic")]
    public byte level;
    public byte job;
    public byte sex;
    public byte[] customized;
    public List<byte> equiped;
    public ushort curExp;
    public ushort totalExp;
    public ushort HP;
    public ushort SP;
    public byte criPro;
    public const float criMag = 1.5f;
    public byte defPoint;
    public float defMag;
    public byte statPoint;
    public ushort atkPoint;
    public ushort gold;
    public bool isDead;
}
