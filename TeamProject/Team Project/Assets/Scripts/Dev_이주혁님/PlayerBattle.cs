using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private bool atkPossible;                  // 공격 가능 여부 표시.          
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
    public void NormalAttack()
    {
        if (atkPossible)
        {                            
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.rotation *= Quaternion.Euler(movement);
                playerAni.Play("Player NormalAttack");                                      
        }                              
    }
    public void Skill_1()
    {
        if (atkPossible)
        {
            Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
            transform.rotation *= Quaternion.Euler(movement);
            playerAni.Play("Player Skill 1");
        }
    }
    public void Skill_2()
    {
        if (atkPossible)
        {
            Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
            transform.rotation *= Quaternion.Euler(movement);
            playerAni.Play("Player Skill 2");
        }
    }
    void FreezePlayer()     
    {
        playerController.enableAct = false;
    }
    void UnFreezePlayer()   
    {
        playerController.enableAct = true;
    }        
    void AtkBlock()
    {
        atkPossible = false;        
    }
    void AtkPossible()
    {
        atkPossible = true;
    }       
    public void Roll()
    {
        
        playerAni.SetBool("isRoll", true);
        CancelInvoke("_Roll");
        Invoke("_Roll", 0.4f);        
    }
    void _Roll()
    {
        playerAni.SetBool("isRoll", false);
    }
    void RollMove()
    {
        
        Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
        transform.rotation *= Quaternion.Euler(movement);
        InvokeRepeating("_RollMove", 0.1f, 0.01f);
        Invoke("Cancel_RollMove", 0.9f);
        
    }
    void _RollMove()
    {
        transform.parent.position = Vector3.Lerp(transform.parent.position, 
            transform.parent.position + transform.forward * 6f, 0.01f);
    }
    void Cancel_RollMove()
    {
        CancelInvoke("_RollMove");
    }
    public void LArmDown()
    {
        playerAni.SetBool("isLArm", true);
    }
    public void LArmUp()
    {
        playerAni.SetBool("isLArm", false);
    }
}

