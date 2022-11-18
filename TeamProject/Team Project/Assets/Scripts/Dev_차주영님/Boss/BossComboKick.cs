using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossComboKick : StateMachineBehaviour
{
    public string Boss_Skill_Effect;
    public string Boss_Skill_Effect2;
    public string Boss_Skill_Effect3;
    public string Boss_Skill_Effect4;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JY_Boss_FireDungeon.s_instance.WeaponEffectOnOff(true);
        InstanceManager.s_instance.PlayBossSkillEffect(Boss_Skill_Effect, 0.5f, JY_Boss_FireDungeon.s_instance.transform);
        InstanceManager.s_instance.PlayBossSkillEffect(Boss_Skill_Effect2, 1f, JY_Boss_FireDungeon.s_instance.transform);
        InstanceManager.s_instance.PlayBossSkillEffect(Boss_Skill_Effect3, 1.5f, JY_Boss_FireDungeon.s_instance.transform);
        InstanceManager.s_instance.PlayBossSkillEffect(Boss_Skill_Effect4, 2f, JY_Boss_FireDungeon.s_instance.transform);
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
