using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private Player player;    
    private float time; // 애니메이션 중 0.1초에서 0.8초 사이에만 이동할 수 있도록 하기 위해 시간을 저장할 변수.
    public float usingStamina;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();     
        player.SetRotate();
        time = 0f;
        animator.GetComponent<Player>().UseStamina(usingStamina);
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
