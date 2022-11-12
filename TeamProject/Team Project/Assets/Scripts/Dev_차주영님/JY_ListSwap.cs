using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_ListSwap : MonoBehaviour
{    
    public List<Button> infoGroup;     
    public List<Button> createButton;
        
    public List<Text> nameTxt;
    public List<Text> infoTxt;

    public List<Image> frameBackground;
    public List<Image> portrait;     
        
    Sprite sourceImg_R;
    Sprite sourceImg_B;
    Sprite warriorM;
    Sprite warriorF;
    Sprite magicianM;
    Sprite magicianF;
    
    private void Awake()
    {
        //소스이미지 로드
        sourceImg_R = Resources.Load<Sprite>("UI_Change/Button Background/Background_r_red");
        sourceImg_B = Resources.Load<Sprite>("UI_Change/Button Background/Background_r_grey");

        warriorM = Resources.Load<Sprite>("UI_Change/Portrait/27");
        warriorF = Resources.Load<Sprite>("UI_Change/Portrait/7");
        magicianM = Resources.Load<Sprite>("UI_Change/Portrait/21");
        magicianF = Resources.Load<Sprite>("UI_Change/Portrait/6");        
    }    

    private void Start()
    {
        PanelSwitch();
    }
        
    void PanelSwitch()
    {               
        for (int i=0; i < 4; i++)
        {
            if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].isNull)
            {
                infoGroup[i].gameObject.SetActive(false);
                createButton[i].gameObject.SetActive(true);
            }
            else
            {
                createButton[i].gameObject.SetActive(false);
                infoGroup[i].gameObject.SetActive(true);
                
                if (i == JY_CharacterListManager.s_instance.selectNum)
                {
                    frameBackground[i].sprite = sourceImg_R;
                }
                else
                {
                    frameBackground[i].sprite = sourceImg_B;
                }
            }
        }
        DataRenew();
    }

    //Character Info의 Text 갱신
    void DataRenew()
    {        
        for(int i = 0; i < 4; i++)
        {
            nameTxt[i].text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].name;
            infoTxt[i].text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].level + "레벨 ";
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].job)
            {
                case EJob.WARRIOR:
                    infoTxt[i].text += "전사";
                    break;
                case EJob.MAGICIAN:
                    infoTxt[i].text += "마법사";
                    break;
                case EJob.NONE:
                    infoTxt[i].text += "무직";
                    break;
                default:
                    break;
            }
            portrait[i].sprite = SwitchPortrait
                (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].gender, 
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].job, i);
        }
    }

    public void DeleteButton(int _num)
    {
        // 캐릭터를 삭제하면 플레이어를 비활성화.
        JY_AvatarLoad.s_instance.origin.gameObject.SetActive(false);
        JY_CharacterListManager.s_instance.DeleteCharacter(_num);
        JY_CharacterListManager.s_instance.selectNum = -1;
        PanelSwitch();
    }

    public void SelectCharacter(int _num)
    {
        //선택 취소 구현
        if (JY_CharacterListManager.s_instance.selectNum == _num)
        {
            JY_AvatarLoad.s_instance.origin.gameObject.SetActive(false);
            JY_CharacterListManager.s_instance.selectNum = -1;
            PanelSwitch();
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = _num;
        JY_AvatarLoad.s_instance.origin.gameObject.SetActive(true);        
        JY_AvatarLoad.s_instance.LoadModelData(_num);        // 선택한 성별 캐릭터 활성화. inventory onenable
        // 선택한 캐릭터의 인벤토리를 카피해옴.
        JY_CharacterListManager.s_instance.CopyInventoryDataToScript(Inventory.instance.items);
        JY_AvatarLoad.s_instance.LobbyDummyClear(JY_CharacterListManager.s_instance.selectNum);
        Inventory.instance.items.ForEach(e =>
        {
            if (e.equipedState.Equals(EquipState.EQUIPED))
            {
                GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + e.image.name);
                GameObject shieldSrc = Resources.Load<GameObject>("Item/Shield/" + e.image.name);
                if (weaponSrc != null)
                    Instantiate<GameObject>(weaponSrc, JY_AvatarLoad.s_instance.charWeaponDummy.transform);
                if (shieldSrc != null)
                    Instantiate<GameObject>(shieldSrc, JY_AvatarLoad.s_instance.charShieldDummy.transform);
            }                       
        });
        PanelSwitch();
    }

    Sprite SwitchPortrait(EGender gender, EJob job, int _num)
    {        
        if (!JY_CharacterListManager.s_instance.jInfoData.infoDataList[_num].isNull)
        {
            if (gender.Equals(EGender.MALE) && job.Equals(EJob.WARRIOR))
                return warriorM;
            else if (gender.Equals(EGender.MALE) && job.Equals(EJob.MAGICIAN))
                return magicianM;
            else if (gender.Equals(EGender.FEMALE) && job.Equals(EJob.WARRIOR))
                return warriorF;
            else
                return magicianF;            
        }
        return null;
    }

}
