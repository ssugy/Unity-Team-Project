using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �κ񿡼��� ���� ��ũ��Ʈ.
public class LobbyUI : MonoBehaviour
{
    [Header("�˾� �޽���")]
    public GameObject noSelectCharacter;
    public GameObject connectServer;

    [Header("��ư")]
    public Button enterWorld;
    public Button quitGame;    

    public Button[] infoGroup;
    public Button[] createChar;
    public Button[] deleteChar;

    [Header("�ؽ�Ʈ")]
    public Text[] nameTxt;
    public Text[] infoTxt;

    [Header("�̹���")]
    public Image[] frameBackground;
    public Image[] portrait;

    [Header("�ҽ� �̹���")]
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
        warriorM = Resources.Load<Sprite>("UI_Change/Portrait/m_warrior");
        warriorF = Resources.Load<Sprite>("UI_Change/Portrait/f_warrior");
        magicianM = Resources.Load<Sprite>("UI_Change/Portrait/m_magician");
        magicianF = Resources.Load<Sprite>("UI_Change/Portrait/f_magician");        
    }

    void Start()
    {
        enterWorld.onClick.AddListener(() => EnterWorld());
        quitGame.onClick.AddListener(() => Application.Quit());

        // for�� ���ο��� ���ٸ� �� ���� �����ؾ� �Ѵ�.
        for (int i = 0; i < 4; i++)
        {
            int j = i;  // i�� ���� ������� �ʱ� ���� ���� ������ ���� ������ ���.
            infoGroup[j].onClick.AddListener(() => SelectCharacter(j));
            createChar[j].onClick.AddListener(() => GameManager.s_instance.LoadScene(3));
            deleteChar[j].onClick.AddListener(() => DeleteCharacter(j));
        }

        PanelSwitch();
    }

    // ������ ĳ���Ͱ� ������ �˾�â�� ���. ������ ĳ���Ͱ� ������ �ش� ĳ���ͷ� ���忡 ����.
    public void EnterWorld()
    {
        if (JY_CharacterListManager.s_instance.selectNum < 0)
        {
            noSelectCharacter.SetActive(true);
            return;
        }
        enterWorld.interactable = false;
        connectServer.SetActive(true);
        NetworkManager.s_instance.Connect();
    }
    
    // �κ� UI ����.
    private void PanelSwitch()
    {
        for (int i = 0; i < 4; i++)
        {
            if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].isNull)
            {
                infoGroup[i].gameObject.SetActive(false);
                createChar[i].gameObject.SetActive(true);
            }
            else
            {
                createChar[i].gameObject.SetActive(false);
                infoGroup[i].gameObject.SetActive(true);
                frameBackground[i].sprite = i == JY_CharacterListManager.s_instance.selectNum
                    ? sourceImg_R : sourceImg_B;                              
            }
        }
        DataRenew();
    }

    // ĳ���� �г� ���� ����.
    private void DataRenew()
    {
        for (int i = 0; i < 4; i++)
        {
            // �� �����̶�� ���� �����͸� ������ �ʿ� ����.
            if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].isNull)
                continue;
            nameTxt[i].text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].name;
            infoTxt[i].text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].level + "���� ";
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].job)
            {
                case EJob.WARRIOR:
                    infoTxt[i].text += "����";
                    break;
                case EJob.MAGICIAN:
                    infoTxt[i].text += "������";
                    break;
                case EJob.NONE:
                    infoTxt[i].text += "����";
                    break;
                default:
                    break;
            }
            portrait[i].sprite = SwitchPortrait
                (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].gender,
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].job, i);
        }
    }

    // �г� �ʻ�ȭ ����.
    private Sprite SwitchPortrait(EGender gender, EJob job, int _num)
    {
        if (!JY_CharacterListManager.s_instance.jInfoData.infoDataList[_num].isNull)
        {
            switch (gender)
            {
                case EGender.FEMALE:
                    switch (job)
                    {
                        case EJob.WARRIOR:
                            return warriorF;
                        case EJob.MAGICIAN:
                            return magicianF;
                        default:
                            return null;
                    }
                case EGender.MALE:
                    switch (job)
                    {
                        case EJob.WARRIOR:
                            return warriorM;
                        case EJob.MAGICIAN:
                            return magicianM;
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }
        return null;
    }

    private void DeleteCharacter(int _num)
    {
        // ĳ���͸� �����ϸ� �÷��̾ ��Ȱ��ȭ.
        JY_AvatarLoad.s_instance.charMale.gameObject.SetActive(false);
        JY_AvatarLoad.s_instance.charFemale.gameObject.SetActive(false);
        JY_CharacterListManager.s_instance.DeleteCharacter(_num);
        JY_CharacterListManager.s_instance.selectNum = -1;
        PanelSwitch();
    }

    private void SelectCharacter(int _num)
    {
        //���� ��� ����
        if (JY_CharacterListManager.s_instance.selectNum == _num)
        {
            JY_CharacterListManager.s_instance.selectNum = -1;
            JY_AvatarLoad.s_instance.charMale.gameObject.SetActive(false);
            JY_AvatarLoad.s_instance.charFemale.gameObject.SetActive(false);            
            PanelSwitch();
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = _num;            
        JY_AvatarLoad.s_instance.LoadModelData(_num);        // ������ ���� ĳ���� Ȱ��ȭ. inventory onenable
        // ������ ĳ������ �κ��丮�� ī���ؿ�.
        JY_CharacterListManager.s_instance.CopyInventoryDataToScript(JY_AvatarLoad.s_instance.inven.items);
        //JY_CharacterListManager.s_instance.CopyInventoryDataToScript(JY_CharacterListManager.s_instance.jInfoData.infoDataList[_num].itemList);
        JY_AvatarLoad.s_instance.inven.items.ForEach(e =>
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
}
