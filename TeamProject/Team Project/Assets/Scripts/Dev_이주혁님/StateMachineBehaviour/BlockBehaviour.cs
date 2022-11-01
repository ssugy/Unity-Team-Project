using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : StateMachineBehaviour
{
    public float curDef;
    public float usingStamina;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curDef = Player.instance.playerStat.defMag;
        Player.instance.enableRecoverSP = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.instance.playerStat.defMag = Shield.shield.defPro;
        Player.instance.UseStamina(usingStamina * Time.deltaTime * 0.3f);
        if (Player.instance.playerStat.CurSP <= 0)
        {
            animator.SetBool("isLArm", false);
        }
        /*if (Player.instance.isAttacked)
        {
            //Player.instance.playerStat.CurSP -= 10f;
            if (Player.instance.playerStat.CurSP <= 0f)
            {
                Player.instance.playerStat.CurSP = 0f;
            }
        }*/

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.instance.playerStat.defMag = curDef;
        Player.instance.enableRecoverSP = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Player.instance.playerStat.defMag = curDef;
        //Player.instance.UseStamina(usingStamina * Time.deltaTime);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}    
}
