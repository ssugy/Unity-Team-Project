using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipOption;

public class Weapon : MonoBehaviour
{
    public int atkPoint;                // ���⺰ ���ݷ�.
    public int level;                   // ���� ������ �÷��̾� ����
    public float atkSpeed;
    public float atkMag;                // �Ϲݰ����� ����� ��, Ȥ�� ��ų�� ����� �� �̸� ���ص� ������ ���⿡ ���Ե�.
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
        // ���⸦ �������� ���
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

    // ���⸦ �������� �� �߰��ɼ��� ������.
    public void ApplyOptions(Item _item)
    {
        if (player == null)
            return;        

        if (_item != null && _item.option != null && _item.option.optionList != null) 
        {
            float value;            
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // ���� ����
                    case EquipOption.EquipAttrib.AttribAtkSpeed:
                        value = _item.option.options[e];
                        atkSpeed += atkSpeed * value / 100.0f;
                        player.playerAni.SetFloat("AtkSpeed", atkSpeed);
                        break;
                    // ���ݷ� ����
                    case EquipOption.EquipAttrib.AttribAtkPoint:
                        value = _item.option.options[e];
                        atkPoint += (int)value;
                        break;
                    // ü��(Health) ����
                    case EquipOption.EquipAttrib.AtrribAtkHealth:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth += (int)value;
                        break;
                    // �ٷ� ����
                    case EquipOption.EquipAttrib.AtrribAtkStrength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength += (int)value;
                        break;
                    // ������ ����
                    case EquipOption.EquipAttrib.AtrribAtkStamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina += (int)value;
                        break;
                    // ��ø ����
                    case EquipOption.EquipAttrib.AtrribAtkDexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity += (int)value;
                        break;
                    // ġ��Ÿ Ȯ�� ����
                    case EquipOption.EquipAttrib.AtrribAtkCriPro:
                        value = _item.option.options[e];
                        player.playerStat.addedCriPro += value;
                        break;
                }
                
            }            
            player.SetState();
        }
    }
    // ���⸦ ���� �������� �� �߰��ɼ��� ������.
    public void ReturnOptions(Item _item)
    {
        if (player == null)
            return;

        if (_item != null && _item.option != null && _item.option.optionList != null)
        {
            float value;            
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // ���ݷ°� ���� �ӵ��� ���⸦ ���� �������� ��
                    // ���� 0, 1f�� �ʱ�ȭ�ǹǷ� ���⼭ �ǵ��� �ʿ� ����.
                    
                    case EquipOption.EquipAttrib.AtrribAtkHealth:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.AtrribAtkStrength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.AtrribAtkStamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.AtrribAtkDexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity -= (int)value;
                        break;                    
                    case EquipOption.EquipAttrib.AtrribAtkCriPro:
                        value = _item.option.options[e];
                        player.playerStat.addedCriPro -= value;
                        break;
                }
            }
            player.SetState();
        }
    }
}
