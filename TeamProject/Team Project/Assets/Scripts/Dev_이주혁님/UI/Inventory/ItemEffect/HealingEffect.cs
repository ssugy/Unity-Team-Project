using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Healing")]
public class HealingEffect : ItemEffect
{
    public int healingPoint;
    public override void ExecuteRole(Item _item)
    {
        Player player;
        if ((player = JY_CharacterListManager.s_instance.playerList[0]) == null)
            return;

        Debug.Log("체력 회복");
        // 회복력 증가 옵션을 감안한 물약 사용
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_POTION);
        InstanceManager.s_instance.ExtraEffectCreate("HealingEffect");
        float total = player.playerStat.addedRecover / 100.0f * healingPoint 
            + healingPoint;
        player.playerStat.CurHP += (int)total;        
    }
}
