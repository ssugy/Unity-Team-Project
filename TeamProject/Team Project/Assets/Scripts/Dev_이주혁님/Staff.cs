using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    public int atkDmg;                    // 마법 공격 사용 시 적용될 공격력.
    public int usingStamina;              // 마법 공격 사용 시 추가로 사용할 SP.
    [HideInInspector] public Player player;
    public Transform shootPoint;
    
    private void OnEnable()
    {                
        player = GetComponentInParent<Player>();
        if (player != null)
        {
            player.staff = this;
            player.shield = null;
            player.SetState();
            if (BattleUI.instance != null)
            {
                if (!Photon.Pun.PhotonNetwork.InRoom)
                    BattleUI.instance.Magic();
                else if(player.photonView.IsMine)
                    BattleUI.instance.Magic();
            }            
        }
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.staff = null;
            player.SetState();
        }        
    }
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
                    // 체력(Health) 증가
                    case EquipOption.EquipAttrib.Health:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth += (int)value;
                        break;
                    // 근력 증가
                    case EquipOption.EquipAttrib.Strength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength += (int)value;
                        break;
                    // 지구력 증가
                    case EquipOption.EquipAttrib.Stamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina += (int)value;
                        break;
                    // 민첩 증가
                    case EquipOption.EquipAttrib.Dexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity += (int)value;
                        break;
                    // 치명타 확률 증가
                    case EquipOption.EquipAttrib.CriPro:
                        value = _item.option.options[e];
                        player.playerStat.addedCriPro += value;
                        break;
                }

            }
            player.SetState();
        }
    }    
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
                    case EquipOption.EquipAttrib.Health:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.Strength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.Stamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.Dexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity -= (int)value;
                        break;
                    case EquipOption.EquipAttrib.CriPro:
                        value = _item.option.options[e];
                        player.playerStat.addedCriPro -= value;
                        break;
                }
            }
            player.SetState();
        }
    }
}
