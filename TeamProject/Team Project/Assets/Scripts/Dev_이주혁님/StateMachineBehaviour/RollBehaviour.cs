using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private Player player;    
    private float time; // �ִϸ��̼� �� 0.1�ʿ��� 0.8�� ���̿��� �̵��� �� �ֵ��� �ϱ� ���� �ð��� ������ ����.
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = Player.instance;     
        player.SetRotate();
        time = 0f;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time>0.1f && time < 0.8f)
        {
            player.RollMove();
        }                  
    }
}
