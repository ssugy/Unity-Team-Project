using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JY_ListSwap : MonoBehaviour
{
    //��� UI GameObject
    public GameObject infoGroup;
    public GameObject createButton;
    public bool check;
    public int listNum;

    //�ؽ�Ʈ ����
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

    //ĳ���� ���� ���ο� ���� Panel�� ButtonSwitch
    //nullcheck = true�� �� CreateButton Ȱ��ȭ
    //nullcheck = false�� �� Character info Ȱ��ȭ
    void panelSwitch(bool nullCheck)
    {
        //ĳ���� ���� check
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
    //Character Info�� Text ����
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
