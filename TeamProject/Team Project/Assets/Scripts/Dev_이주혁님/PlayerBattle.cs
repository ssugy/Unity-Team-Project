using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private bool atkPossible;                  // ���� ���� ���� ǥ��.
    public CharacterController playerController;
    private Animator playerAni;                // �÷��̾��� �ִϸ�����.            
    private PlayerMove playerMove;             // enableAct�� �����ϱ� ���� ������.    
    private FixedJoystick playerJoysitck;      // ���̽�ƽ �Է��� �޾ƿ�.
    public Transform rWeaponDummy;              // ������ ���� ����.
    public TrailRenderer rWeaponEffect;        // ������ ���� ����Ʈ. (�˱�)
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerAni = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        playerJoysitck = FixedJoystick.instance;
        atkPossible = true;
        rWeaponEffect = rWeaponDummy.GetChild(0).GetChild(2).GetComponent<TrailRenderer>();
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
        playerMove.enableAct = false;        
    }
    void UnFreezePlayer()   
    {
        playerMove.enableAct = true;
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
        InvokeRepeating("_RollMove", 0.1f, Time.deltaTime);
        Invoke("Cancel_RollMove", 0.9f);
        
    }
    void _RollMove()
    {             
        playerController.Move(transform.forward * 6f *
            Time.deltaTime + new Vector3(0, playerMove.gravity * Time.deltaTime, 0));
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
    public void WeaponEffectOn()
    {
        rWeaponEffect.emitting = true;
    }
    public void WeaponEffectOff()
    {
        rWeaponEffect.emitting = false;
    }
}

