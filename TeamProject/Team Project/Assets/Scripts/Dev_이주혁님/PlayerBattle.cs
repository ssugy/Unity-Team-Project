using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private bool atkPossible;                  // 공격 가능 여부 표시.        
    private int comboStep;                     // 콤보 진행 단계.
    private Transform camAxis;                 // 카메라 중심축의 forward를 가져오기 위해 변수를 선언.       
    private Animator playerAni;                // 플레이어의 애니메이터.            
    private PlayerController playerController; // enableAct를 전달하기 위해 선언함.    
    private FixedJoystick playerJoysitck;       // 조이스틱 입력을 받아옴.
    void Start()
    {
        camAxis = Camera.main.transform.parent;
        playerAni = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerJoysitck = FixedJoystick.instance;
        atkPossible = true;
    }           
    void ResetCombo()
    {
        atkPossible = true;        
        comboStep = 0;
        playerAni.SetBool("isCombo", false);
    }   
    void NormalAttack()
    {
        if (atkPossible)
        {
            if (comboStep == 0)
            {
                transform.forward = new Vector3(camAxis.forward.x, 0, camAxis.forward.z);
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.forward += movement;
                playerAni.Play("Player Attack 1");                
            }
            else if (comboStep == 1)
            {
                transform.forward = new Vector3(camAxis.forward.x, 0, camAxis.forward.z);
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.forward += movement;                             
            }
            else
            {
                transform.forward = new Vector3(camAxis.forward.x, 0, camAxis.forward.z);
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.forward += movement;                              
            }
        }        
        playerAni.SetBool("isCombo", true);
               
    }
    void FreezePlayer()     
    {
        playerController.enableAct = false;
    }
    void UnFreezePlayer()   
    {
        playerController.enableAct = true;
    }    
    void ComboStack()
    {
        comboStep++;
    }
    void AtkBlock()
    {
        atkPossible = false;        
    }
    void AtkPossible()
    {
        atkPossible = true;
    }   
    void ComboBlock()
    {
        playerAni.SetBool("isCombo", false);
    }
}

