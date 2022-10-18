using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JY_ListSwap : MonoBehaviour
{
    //출력 UI GameObject
    public GameObject infoGroup;
    public GameObject createButton;
    public bool check;
    public int listNum;

    //텍스트 변경
    public Text nameTxt;
    public Text levelTxt;
    public Text jobTxt;
    public Text speciesTxt;

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

    public Inventory setInventory;
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
        panelSwitch(check);
    }

    //캐릭터 생성 여부에 따른 Panel과 ButtonSwitch
    //nullcheck = true일 때 CreateButton 활성화
    //nullcheck = false일 때 Character info 활성화
    void panelSwitch(bool nullCheck)
    {
          //캐릭터 생성 check
        check = JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].isNull;

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
        string leveltxt;

        nameTxt.text = JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].name;
        if(JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].level < 10)
        {
            leveltxt = "0" + JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].level.ToString();
        }
        else
        {
            leveltxt = JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].level.ToString();
        }
        levelTxt.text = leveltxt;
        jobTxt.text = JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].job;
        jobTxt.text = JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].job;
        portraitImage.sprite = switchPortrait(JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].gender,
                                              JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].job);
        speciesTxt.text = JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].species;
    }

    public void deleteButton()
    {
        if (listNum == JY_CharacterListManager.s_instance.selectNum)
            JY_AvatarLoad.s_instance.origin.SetActive(false);
        JY_CharacterListManager.s_instance.deleteCharacter(listNum);
        JY_CharacterListManager.s_instance.selectNum = -1;
        
    }

    public void selectCharacter()
    {
        //선택 취소 구현
        if (JY_CharacterListManager.s_instance.selectNum == listNum)
        {
            JY_AvatarLoad.s_instance.origin.SetActive(false);
            JY_CharacterListManager.s_instance.selectNum = -1;
            JY_CharacterListManager.s_instance.selectPortrait = null;
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = listNum;
        JY_AvatarLoad.s_instance.origin.SetActive(true);
        JY_AvatarLoad.s_instance.LoadModelData(listNum);
        //Inventory.instance.items.Clear();
        Inventory.instance.items = JY_CharacterListManager.s_instance.characterInventoryData.InventoryJDataList[JY_CharacterListManager.s_instance.selectNum].itemList;
        JY_CharacterListManager.s_instance.selectPortrait =  switchPortrait(JY_CharacterListManager.instance.characterData.infoDataList[listNum].gender,
                                              JY_CharacterListManager.instance.characterData.infoDataList[listNum].job);;
    }

    Sprite switchPortrait(string gender, string job)
    {
        Sprite source;
        if (!JY_CharacterListManager.s_instance.characterData.infoDataList[listNum].isNull)
        {
            if (gender.Equals("M") && job.Equals("전사"))
                source = warriorM;
            else if (gender.Equals("M") && job.Equals("마법사"))
                source = magicianM;
            else if (gender.Equals("F") && job.Equals("전사"))
                source = warriorF;
            else
                source = magicianF;
            return source;
        }
        return null;
    }
}
