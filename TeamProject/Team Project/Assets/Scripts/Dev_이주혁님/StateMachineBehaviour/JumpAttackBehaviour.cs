using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackBehaviour : StateMachineBehaviour
{
    private Player player;
    private Weapon weapon;
    public float atkMag;
    private float time; // 애니메이션 중 0.1초에서 0.8초 사이에만 이동할 수 있도록 하기 위해 시간을 저장할 변수.


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = Player.instance;
        weapon = Weapon.weapon;
        weapon.atkMag = atkMag;
        time = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time > 0.1f && time < 1.4f)
        {
            player.controller.Move(animator.transform.forward * 2f *
            Time.deltaTime + new Vector3(0, player.gravity * Time.deltaTime, 0));
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
