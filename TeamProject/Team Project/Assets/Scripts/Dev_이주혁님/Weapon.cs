using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipOption;

public class Weapon : MonoBehaviour
{
    public int atkPoint;                // 무기별 공격력.
    public int level;                   // 장착 가능한 플레이어 레벨
    public float atkSpeed;
    public float atkMag;                // 일반공격을 사용할 때, 혹은 스킬을 사용할 때 미리 정해둔 배율이 여기에 대입됨.
    public Player player;    
    public BoxCollider weaponHitbox;

    private void OnEnable()
    {                      
        player = GetComponentInParent<Player>();                               
        weaponHitbox = GetComponentInChildren<BoxCollider>();
        if (player != null && player.playerAni != null)
        {
            player.playerAni.SetFloat("AtkSpeed", atkSpeed);
        }
        if (player != null)
        {
            player.rWeapon = this;
            player.SetState();
        }            
    }    
    private void OnDisable()
    {
        // 무기를 해제했을 경우
        weaponHitbox = null;
        if (player != null && player.playerAni != null)
            player.playerAni.SetFloat("AtkSpeed", 1f);
        if (player != null)
        {
            player.rWeapon = null;
            player.SetState();
        }        
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Enemy"))
        {
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_ATTACK);
            player.Attack(other);                       
        }
    }

    public void ApplyOptions(Item _item)
    {
        float value = 0f;

        if (_item != null && _item.option != null && _item.option.optionList != null) 
        {
            //player.playerStat.CopyToTemp();
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // 공속 증가
                    case EquipOption.EquipAttrib.AttribAtkSpeed:
                        value = _item.option.options[e];
                        atkSpeed += atkSpeed * value / 100.0f;
                        player.playerAni.SetFloat("AtkSpeed", atkSpeed);
                        break;
                    // 공격력 증가
                    case EquipOption.EquipAttrib.AttribAtkPower:
                        value = _item.option.options[e];
                        atkPoint += (int)value;
                        break;
                    // 체력(Health) 증가
                    case EquipOption.EquipAttrib.AtrribAtkHP:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpHealth += (int)value;
                        }
                        break;
                    // 근력 증가
                    case EquipOption.EquipAttrib.AtrribAtkStrength:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStrength += (int)value;
                        }
                        break;
                    // 스태미나 증가
                    case EquipOption.EquipAttrib.AtrribAtkStamina:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStamina += (int)value;
                        }
                        break;
                    // 민첩 증가
                    case EquipOption.EquipAttrib.AtrribAtkDex:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpDexterity += (int)value;
                        }
                        break;
                    // 치명타 확률 증가
                    case EquipOption.EquipAttrib.AtrribAtkCritical:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedCriticalPro += value;
                        }
                        break;
                }
                // temp값을 이용해 플레이어 능력치를 새로 계산
                player.SetStateOption();
            }
        }
    }
}
