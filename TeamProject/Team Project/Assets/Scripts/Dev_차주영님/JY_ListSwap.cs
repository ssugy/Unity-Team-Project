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
    public Text nameTxt;
    public Text levelTxt;
    public Text jobTxt;
    public Text speciesTxt;

    //ĳ���� ���ý�/������ �ε�� ���� UI
    public Image frameBackground;
    public Image portraitImage;
    
    //�ҽ� �̹��� 
    Sprite sourceImg_R;
    Sprite sourceImg_B;
    Sprite warriorM;
    Sprite warriorF;
    Sprite magicianM;
    Sprite magicianF;

    public Inventory setInventory;
    private void Awake()
    {
        //�ҽ��̹��� �ε�
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

    //ĳ���� ���� ���ο� ���� Panel�� ButtonSwitch
    //nullcheck = true�� �� CreateButton Ȱ��ȭ
    //nullcheck = false�� �� Character info Ȱ��ȭ
    void panelSwitch(bool nullCheck)
    {
          //ĳ���� ���� check
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
    //Character Info�� Text ����
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
        //���� ��� ����
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
            if (gender.Equals("M") && job.Equals("����"))
                source = warriorM;
            else if (gender.Equals("M") && job.Equals("������"))
                source = magicianM;
            else if (gender.Equals("F") && job.Equals("����"))
                source = warriorF;
            else
                source = magicianF;
            return source;
        }
        return null;
    }
}
