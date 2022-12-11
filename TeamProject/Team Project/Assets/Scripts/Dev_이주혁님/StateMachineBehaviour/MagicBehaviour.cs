using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 방패 방어 애니매이션 스크립트
public class MagicBehaviour : StateMachineBehaviour
{    
    public float usingStamina;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        usingStamina += animator.GetComponent<Player>().staff.usingStamina;
        animator.GetComponent<Player>().UseStamina(usingStamina);
    }
    

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        //Player.instance.playerStat.defMag = curDef;
        //Player.instance.UseStamina(usingStamina * Time.deltaTime);
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}    
}
