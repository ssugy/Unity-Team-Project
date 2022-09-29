using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackBehaviour : StateMachineBehaviour
{
    private BoxCollider weaponHitbox;
    private Weapon weapon;
    private float time;

    public float atkMag;
    public float startFrame;
    public float endFrame;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponHitbox = Weapon.weapoonHitbox;
        weapon = Weapon.weapon;
        time = 0f;
        weapon.atkMag = atkMag;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time > startFrame && time < endFrame)
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
