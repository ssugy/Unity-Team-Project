using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackBehaviour : StateMachineBehaviour
{
    BoxCollider weaponHitbox;
    Weapon weapon;
    float time;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponHitbox = PlayerController.player.GetComponent<PlayerController>().rWeaponDummy.GetComponentInChildren<BoxCollider>();
        weapon = weaponHitbox.transform.GetComponent<Weapon>();
        time = 0f;
        weapon.atkMag = 1.05f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time > 0.28f && time < 0.44f)
        {
            weaponHitbox.enabled = true;
        }
        else
        {
            weaponHitbox.enabled = false;
        }
    }

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
