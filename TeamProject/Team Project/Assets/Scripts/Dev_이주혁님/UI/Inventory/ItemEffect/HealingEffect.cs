using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Healing")]
public class HealingEffect : ItemEffect
{
    public int healingPoint;
    public override void ExecuteRole(Item _item)
    {
        Debug.Log("체력 회복");
        // 회복력 증가 옵션을 감안한 물약 사용
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_POTION);
        InstanceManager.s_instance.ExtraEffectCreate("HealingEffect");
        //float total = (float)JY_CharacterListManager.s_instance.playerList[0].playerStat.addedRecover / 100.0f * healingPoint;
        //JY_CharacterListManager.s_instance.playerList[0].playerStat.CurHP += (int)total;        
        JY_CharacterListManager.s_instance.playerList[0].playerStat.CurHP += healingPoint;
        Debug.Log(healingPoint);
    }
}
