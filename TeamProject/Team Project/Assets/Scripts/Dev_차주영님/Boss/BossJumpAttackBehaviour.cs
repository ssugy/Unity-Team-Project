using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJumpAttackBehaviour : StateMachineBehaviour
{
    public string Boss_Skill_Effect;
    public string Boss_Skill_Effect2;
    public string Boss_Skill_Effect3;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JY_Boss_FireDungeon.s_instance.WeaponEffectOnOff(true);
        InstanceManager.s_instance.PlayBossSkillEffect(Boss_Skill_Effect2, 2f, JY_Boss_FireDungeon.s_instance.transform);
        AudioManager.s_instance.CallSFXPlay(AudioManager.SOUND_NAME.Boss_JUMP,2f);
        JY_ShakeCamera cam = Camera.main.GetComponent<JY_ShakeCamera>();
        cam.onSakeCamera();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JY_Boss_FireDungeon.s_instance.BossRotate();
        JY_Boss_FireDungeon.s_instance.WeaponEffectOnOff(false);
        JY_Boss_FireDungeon.s_instance.MeleeAreaDisEnable();
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
