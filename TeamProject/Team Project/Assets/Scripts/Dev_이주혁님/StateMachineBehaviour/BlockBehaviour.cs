using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 방패 방어 애니매이션 스크립트
public class BlockBehaviour : StateMachineBehaviour
{
    public float curDef;
    public float usingStamina;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curDef = animator.GetComponent<Player>().playerStat.defMag;
        animator.GetComponent<Player>().enableRecoverSP = false;
        animator.GetComponent<Player>().isGaurd = true;
        animator.GetComponent<Player>().playerStat.defMag = animator.GetComponent<Player>().shield.defPro;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        animator.GetComponent<Player>().UseStamina(usingStamina * Time.deltaTime * 0.3f);
        if (animator.GetComponent<Player>().playerStat.CurSP <= 0)
            animator.GetComponent<Player>().LArmUp();

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Player>().playerStat.defMag = curDef;
        animator.GetComponent<Player>().enableRecoverSP = true;
        animator.GetComponent<Player>().isGaurd = false;
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
