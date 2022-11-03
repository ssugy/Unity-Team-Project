using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStrikeBehaviour : StateMachineBehaviour
{
    private Weapon weapon;
    public float atkMag;
    public float usingStamina;
    public string EffectName1;
    public string EffectName2;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weapon = Weapon.weapon;
        weapon.atkMag = atkMag;
        Player.instance.UseStamina(usingStamina);
        InstanceManager.s_instance.PlaySkillEffect(EffectName1,0.5f);
        InstanceManager.s_instance.NormalAttackEffect("Normal_Attack_Effect3");
        InstanceManager.s_instance.PlaySkillEffect(EffectName2,0.6f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
