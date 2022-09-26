using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private PlayerController playerController;    
    private float time; // �ִϸ��̼� �� 0.1�ʿ��� 0.8�� ���̿��� �̵��� �� �ֵ��� �ϱ� ���� �ð��� ������ ����.
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController = animator.transform.GetComponent<PlayerController>();
        /*Vector3 movement = new Vector3(FixedJoystick.instance.Horizontal, 0,
                FixedJoystick.instance.Vertical);
        animator.transform.rotation *= Quaternion.Euler(movement);*/
        playerController.SetRotate();
        time = 0f;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time>0.1f && time < 0.8f)
        {
            playerController.RollMove();
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
