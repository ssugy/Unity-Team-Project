using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_ListSwap : MonoBehaviour
{
    //출력 UI GameObject
    public GameObject infoGroup;
    public GameObject createButton;
    public bool check;
    public int listNum;

    //텍스트 변경
    public Text nameTxt;
    public Text infoTxt;       

    //캐릭터 선택시/데이터 로드시 변경 UI
    public Image frameBackground;
    public Image portraitImage;    

    //소스 이미지 
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

    
    void Update()
    {
        PanelSwitch(check);
    }

    //캐릭터 생성 여부에 따른 Panel과 ButtonSwitch
    //nullcheck = true일 때 CreateButton 활성화
    //nullcheck = false일 때 Character info 활성화
    void PanelSwitch(bool nullCheck)
    {
        //캐릭터 생성 check
        check = JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].isNull;

        if (nullCheck)
        {
            infoGroup.SetActive(false);
            createButton.SetActive(true);
        }
        else
        {
            createButton.SetActive(false);
            infoGroup.SetActive(true);
            DataRenew();
            if (listNum == JY_CharacterListManager.s_instance.selectNum)
            {
                frameBackground.sprite = sourceImg_R;
            }
            else
            {
                frameBackground.sprite = sourceImg_B;
            }
        }
    }
    //Character Info의 Text 갱신
    void DataRenew()
    {        
        nameTxt.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].name;
        infoTxt.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].level + "레벨 ";        
        switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].job)
        {
            case EJob.WARRIOR:
                infoTxt.text += "전사";
                break;
            case EJob.MAGICIAN:
                infoTxt.text += "마법사";
                break;
            case EJob.NONE:
                infoTxt.text += "무직";
                break;
            default:
                break;
        }        
        portraitImage.sprite = SwitchPortrait(JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender,
                                              JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].job);
    }

    public void deleteButton()
    {
        // 캐릭터를 삭제하면 플레이어를 비활성화.
        JY_AvatarLoad.s_instance.origin.gameObject.SetActive(false);
        JY_CharacterListManager.s_instance.DeleteCharacter(listNum);
        JY_CharacterListManager.s_instance.selectNum = -1;       
    }

    public void SelectCharacter()
    {
        //선택 취소 구현
        if (JY_CharacterListManager.s_instance.selectNum == listNum)
        {
            JY_AvatarLoad.s_instance.origin.gameObject.SetActive(false);
            JY_CharacterListManager.s_instance.selectNum = -1;            
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = listNum;
        JY_AvatarLoad.s_instance.origin.gameObject.SetActive(true);        
        JY_AvatarLoad.s_instance.LoadModelData(listNum);        // 선택한 성별 캐릭터 활성화. inventory onenable
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
    }

    Sprite SwitchPortrait(EGender gender, EJob job)
    {        
        if (!JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].isNull)
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
