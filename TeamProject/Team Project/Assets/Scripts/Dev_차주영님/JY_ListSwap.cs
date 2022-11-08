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
        PanelSwitch(check);
    }

    //ĳ���� ���� ���ο� ���� Panel�� ButtonSwitch
    //nullcheck = true�� �� CreateButton Ȱ��ȭ
    //nullcheck = false�� �� Character info Ȱ��ȭ
    void PanelSwitch(bool nullCheck)
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
        infoTxt.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].level + "���� ";        
        switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].job)
        {
            case EJob.WARRIOR:
                infoTxt.text += "����";
                break;
            case EJob.MAGICIAN:
                infoTxt.text += "������";
                break;
            case EJob.NONE:
                infoTxt.text += "����";
                break;
            default:
                break;
        }        
        portraitImage.sprite = SwitchPortrait(JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender,
                                              JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].job);
    }

    public void deleteButton()
    {
        // ĳ���͸� �����ϸ� �÷��̾ ��Ȱ��ȭ.
        JY_AvatarLoad.s_instance.origin.gameObject.SetActive(false);
        JY_CharacterListManager.s_instance.DeleteCharacter(listNum);
        JY_CharacterListManager.s_instance.selectNum = -1;       
    }

    public void SelectCharacter()
    {
        //���� ��� ����
        if (JY_CharacterListManager.s_instance.selectNum == listNum)
        {
            JY_AvatarLoad.s_instance.origin.gameObject.SetActive(false);
            JY_CharacterListManager.s_instance.selectNum = -1;            
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = listNum;
        JY_AvatarLoad.s_instance.origin.gameObject.SetActive(true);        
        JY_AvatarLoad.s_instance.LoadModelData(listNum);        // ������ ���� ĳ���� Ȱ��ȭ. inventory onenable
        // ������ ĳ������ �κ��丮�� ī���ؿ�.
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
