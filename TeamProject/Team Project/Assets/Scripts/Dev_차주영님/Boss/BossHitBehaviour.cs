using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JY_Boss_FireDungeon.s_instance.StopAllCoroutines();
        JY_Boss_FireDungeon.s_instance.WeaponEffectOnOff(false);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_HIT);
        JY_Boss_FireDungeon.s_instance.MeleeAreaDisEnable();
        InstanceManager.s_instance.StopAllBossEffect();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JY_Boss_FireDungeon.s_instance.HitSkillNum = -1;
        JY_Boss_FireDungeon.s_instance.UnfreezeBoss();
        JY_Boss_FireDungeon.s_instance.isAttack = false;
        JY_Boss_FireDungeon.s_instance.HitIntermission(3f);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
