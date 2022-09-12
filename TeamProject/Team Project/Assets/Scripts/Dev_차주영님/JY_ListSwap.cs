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
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI jobTxt;
    public TextMeshProUGUI speciesTxt;

    private void Start()
    {
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
        check = JY_CharacterListManager.instance.characterData.infoDataList[listNum].isNull;

        if (nullCheck)
        {
            infoGroup.SetActive(false);
            createButton.SetActive(true);

        }
        else
        {
            createButton.SetActive(false);
            infoGroup.SetActive(true);
            textDataRenew();
        }
    }
    //Character Info의 Text 갱신
    void textDataRenew()
    {
        string leveltxt;

        nameTxt.text = JY_CharacterListManager.instance.characterData.infoDataList[listNum].name;
        if(JY_CharacterListManager.instance.characterData.infoDataList[listNum].level < 10)
        {
            leveltxt = "0" + JY_CharacterListManager.instance.characterData.infoDataList[listNum].level.ToString();
        }
        else
        {
            leveltxt = JY_CharacterListManager.instance.characterData.infoDataList[listNum].level.ToString();
        }
        levelTxt.text = leveltxt;
        jobTxt.text = JY_CharacterListManager.instance.characterData.infoDataList[listNum].job;
        speciesTxt.text = JY_CharacterListManager.instance.characterData.infoDataList[listNum].species;
    }
}
