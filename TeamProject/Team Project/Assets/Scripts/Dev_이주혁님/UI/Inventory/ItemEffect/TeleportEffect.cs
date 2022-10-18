using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Teleport")]
public class TeleportEffect : ItemEffect
{
    public int sceneNum;
    public override void ExecuteRole(Item _item)
    {
        GameManager.s_instance.LoadScene(sceneNum);
    }
}
