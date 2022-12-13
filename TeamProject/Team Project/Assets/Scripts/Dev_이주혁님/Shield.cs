using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{    
    public float defPro;                    // ���� ��� �� ����� �����.    
    [HideInInspector] public Player player; 
    
    private void OnEnable()
    {        
        player = GetComponentInParent<Player>();
        if (player != null)
        {
            player.shield = this;
            player.staff = null;
            player.SetState();

            if (BattleUI.instance != null)
            {
                if (!Photon.Pun.PhotonNetwork.InRoom)
                    BattleUI.instance.Guard();
                else if (player.photonView.IsMine)
                    BattleUI.instance.Guard();
            }
        }
        
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.shield = null;
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
                    // ���� ����
                    case EquipOption.EquipAttrib.DefPoint:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint += (int)value;
                        break;
                    // HP ����
                    case EquipOption.EquipAttrib.HP:
                        value = _item.option.options[e];
                        player.playerStat.addedHP += (int)value;
                        break;
                    // SP ����
                    case EquipOption.EquipAttrib.SP:
                        value = _item.option.options[e];
                        player.playerStat.addedSP += (int)value;
                        break;
                    // ü�� ����
                    case EquipOption.EquipAttrib.Health:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth += (int)value;
                        break;
                    // �ٷ� ����
                    case EquipOption.EquipAttrib.Strength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength += (int)value;
                        break;
                    // ������ ����
                    case EquipOption.EquipAttrib.Stamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina += (int)value;
                        break;
                    // ��ø ����
                    case EquipOption.EquipAttrib.Dexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity += (int)value;
                        break;
                    // ���� ȿ�� ����
                    case EquipOption.EquipAttrib.PotionRecover:
                        value = _item.option.options[e];
                        player.playerStat.addedRecover += (int)value;
                        break;
                    // ȸ���� ����
                    case EquipOption.EquipAttrib.AvoidPro:
                        value = _item.option.options[e];
                        player.playerStat.addedAvoid += (int)value;
                        break;
                    // ��� �� ������ �߰� ���� (1~20�ۼ�Ʈ ����)
                    case EquipOption.EquipAttrib.ShieldDef:
                        value = _item.option.options[e];
                        defPro += value / 100.0f;
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
                    case EquipOption.EquipAttrib.DefPoint:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint -= (int)value;
                        break;
                    // HP ����
                    case EquipOption.EquipAttrib.HP:
                        value = _item.option.options[e];
                        player.playerStat.addedHP -= (int)value;
                        break;
                    // SP ����
                    case EquipOption.EquipAttrib.SP:
                        value = _item.option.options[e];
                        player.playerStat.addedSP -= (int)value;
                        break;
                    // ü�� ����
                    case EquipOption.EquipAttrib.Health:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth -= (int)value;
                        break;
                    // �ٷ� ����
                    case EquipOption.EquipAttrib.Strength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength -= (int)value;
                        break;
                    // ������ ����
                    case EquipOption.EquipAttrib.Stamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina -= (int)value;
                        break;
                    // ��ø ����
                    case EquipOption.EquipAttrib.Dexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity -= (int)value;
                        break;
                    // ���� ȿ�� ����
                    case EquipOption.EquipAttrib.PotionRecover:
                        value = _item.option.options[e];
                        player.playerStat.addedRecover -= (int)value;
                        break;
                    // ȸ���� ����
                    case EquipOption.EquipAttrib.AvoidPro:
                        value = _item.option.options[e];
                        player.playerStat.addedAvoid -= (int)value;
                        break;                    
                }

            }
            player.SetState();
        }
    }
}
