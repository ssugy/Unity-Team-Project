using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private bool atkPossible;                  // ���� ���� ���� ǥ��.        
    private int comboStep;                     // �޺� ���� �ܰ�.
    private Transform camAxis;                 // ī�޶� �߽����� forward�� �������� ���� ������ ����.       
    private Animator playerAni;                // �÷��̾��� �ִϸ�����.            
    private PlayerController playerController; // enableAct�� �����ϱ� ���� ������.    
    private FixedJoystick playerJoysitck;       // ���̽�ƽ �Է��� �޾ƿ�.
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
    public void NormalAttack()
    {
        if (atkPossible)
        {
            if (comboStep == 0)
            {
                //transform.forward = new Vector3(camAxis.forward.x, 0, camAxis.forward.z);
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.rotation *= Quaternion.Euler(movement);
                playerAni.Play("Player Attack 1");                
            }
            /*else if (comboStep == 1)
            {
                //transform.forward = new Vector3(camAxis.forward.x, 0, camAxis.forward.z);
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.forward += movement;                             
            }*/
            else
            {
                //transform.forward = new Vector3(camAxis.forward.x, 0, camAxis.forward.z);
                Vector3 movement = new Vector3(playerJoysitck.Horizontal, 0,
                playerJoysitck.Vertical);
                transform.rotation *= Quaternion.Euler(movement);
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
}

