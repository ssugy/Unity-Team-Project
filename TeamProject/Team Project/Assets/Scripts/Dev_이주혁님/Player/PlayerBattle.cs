using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private bool atkPossible;                  // 공격 가능 여부 표시.
    public CharacterController playerController;
    private Animator playerAni;                // 플레이어의 애니메이터.            
    private PlayerMove playerMove;             // enableAct를 전달하기 위해 선언함.    
    private FixedJoystick playerJoysitck;      // 조이스틱 입력을 받아옴.
    public Transform rWeaponDummy;              // 오른손 무기 더미.
    public TrailRenderer rWeaponEffect;        // 오른손 무기 이펙트. (검기)
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
    public void RollMove()
    {                
        playerController.Move(transform.forward * 6f *
            Time.deltaTime + new Vector3(0, playerMove.gravity * Time.deltaTime, 0));           }
    
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
    public void Die()
    {            
        Camera.main.GetComponent<MainCamController>().enabled = false;   // 플레이어가 사망하면 더 이상 카메라가 움직이지 않게 함.    
        Camera.main.GetComponent<WhenPlayerDie>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Die"))
        {
            Die();
        }
    }
}

