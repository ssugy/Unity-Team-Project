using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private bool atkPossible;                  // ���� ���� ���� ǥ��.
    private Rigidbody playerrb;                // �÷��̾��� ������ٵ�
    private Animator playerAni;                // �÷��̾��� �ִϸ�����.            
    private PlayerController playerController; // enableAct�� �����ϱ� ���� ������.    
    private FixedJoystick playerJoysitck;      // ���̽�ƽ �Է��� �޾ƿ�.
    void Start()
    {
        playerrb = GetComponent<Rigidbody>();
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
        playerrb.velocity += new Vector3(0, -9.8f, 0);
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
        playerrb.velocity = transform.forward *6f;        
    }
    void Cancel_RollMove()
    {
        CancelInvoke("_RollMove");
        playerrb.velocity = Vector3.zero;
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

