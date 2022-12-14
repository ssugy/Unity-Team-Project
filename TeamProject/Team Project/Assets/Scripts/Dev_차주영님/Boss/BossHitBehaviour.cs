using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JY_Boss_FireDungeon.s_instance.WeaponEffectOnOff(false);
        InstanceManager.s_instance.StopAllBossEffect();
    }
}
