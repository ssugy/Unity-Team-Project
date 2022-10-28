using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_ListSwap : MonoBehaviour
{
    //��� UI GameObject
    public GameObject infoGroup;
    public GameObject createButton;
    public bool check;
    public int listNum;

    //�ؽ�Ʈ ����
    public Text nameTxt;
    public Text infoTxt;       

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
    //Character Info�� Text ����
    void DataRenew()
    {        
        nameTxt.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].name;
        infoTxt.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].level + "���� " + JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].job;        
        portraitImage.sprite = switchPortrait(JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender,
                                              JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].job);        
    }

    public void deleteButton()
    {
        if (listNum == JY_CharacterListManager.s_instance.selectNum)
            JY_AvatarLoad.s_instance.origin.SetActive(false);
        JY_CharacterListManager.s_instance.DeleteCharacter(listNum);
        JY_CharacterListManager.s_instance.selectNum = -1;

    }

    public void selectCharacter()
    {
        //���� ��� ����
        if (JY_CharacterListManager.s_instance.selectNum == listNum)
        {
            JY_AvatarLoad.s_instance.origin.SetActive(false);
            JY_CharacterListManager.s_instance.selectNum = -1;            
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = listNum;
        JY_AvatarLoad.s_instance.origin.SetActive(true);        
        JY_AvatarLoad.s_instance.LoadModelData(listNum);        // ������ ���� ĳ���� Ȱ��ȭ. inventory onenable
        // ������ ĳ������ �κ��丮�� ī���ؿ�.
        JY_CharacterListManager.s_instance.CopyInventoryDataToScript(Inventory.instance.items);
        JY_AvatarLoad.s_instance.LobbyDummyClear(JY_CharacterListManager.s_instance.selectNum);
        foreach (Item one in Inventory.instance.items)
        {
            if (one.equipedState.Equals(EquipState.EQUIPED))
            {
                one.effects[0].ExecuteRole(one);                
            }
        }        
    }

    Sprite switchPortrait(string gender, string job)
    {
        Sprite source;
        if (!JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].isNull)
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
