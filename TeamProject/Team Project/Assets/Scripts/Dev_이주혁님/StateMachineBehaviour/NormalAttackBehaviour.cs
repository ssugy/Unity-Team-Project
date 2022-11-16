using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NormalAttackBehaviour : StateMachineBehaviour
{    
    private Weapon weapon;    
    public float atkMag;
    public float usingStamina;
    public string EffectName;
    public int SkillNum;
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        weapon = animator.GetComponent<Player>().rWeapon;
        weapon.atkMag = atkMag;
        animator.GetComponent<Player>().UseStamina(usingStamina);
        animator.GetComponent<Player>().WEOn();
        InstanceManager.s_instance.NormalAttackEffect(EffectName);
        if (JY_Boss_FireDungeon.s_instance != null)
            JY_Boss_FireDungeon.s_instance.HitSkillNum = SkillNum;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //}
   

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
