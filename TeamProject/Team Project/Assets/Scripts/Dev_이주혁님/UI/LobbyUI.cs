using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 로비에서만 사용될 스크립트.
public class LobbyUI : MonoBehaviour
{
    [Header("팝업 메시지")]
    public GameObject noSelectCharacter;
    public GameObject connectServer;

    [Header("버튼")]
    public Button enterWorld;
    public Button quitGame;    

    public Button[] infoGroup;
    public Button[] createChar;
    public Button[] deleteChar;

    [Header("텍스트")]
    public Text[] nameTxt;
    public Text[] infoTxt;

    [Header("이미지")]
    public Image[] frameBackground;
    public Image[] portrait;

    [Header("소스 이미지")]
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
        warriorM = Resources.Load<Sprite>("UI_Change/Portrait/m_warrior");
        warriorF = Resources.Load<Sprite>("UI_Change/Portrait/f_warrior");
        magicianM = Resources.Load<Sprite>("UI_Change/Portrait/m_magician");
        magicianF = Resources.Load<Sprite>("UI_Change/Portrait/f_magician");        
    }

    void Start()
    {
        enterWorld.onClick.AddListener(() => EnterWorld());
        quitGame.onClick.AddListener(() => Application.Quit());

        // for문 내부에서 람다를 쓸 때는 주의해야 한다.
        for (int i = 0; i < 4; i++)
        {
            int j = i;  // i를 직접 사용하지 않기 위해 지역 변수에 값을 복사해 사용.
            infoGroup[j].onClick.AddListener(() => SelectCharacter(j));
            createChar[j].onClick.AddListener(() => GameManager.s_instance.LoadScene(3));
            deleteChar[j].onClick.AddListener(() => DeleteCharacter(j));
        }

        PanelSwitch();
    }

    // 선택한 캐릭터가 없으면 팝업창을 출력. 선택한 캐릭터가 있으면 해당 캐릭터로 월드에 접속.
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
    
    // 로비 UI 갱신.
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

    // 캐릭터 패널 정보 갱신.
    private void DataRenew()
    {
        for (int i = 0; i < 4; i++)
        {
            // 빈 슬롯이라면 굳이 데이터를 갱신할 필요 없음.
            if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].isNull)
                continue;
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

    // 패널 초상화 갱신.
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
        // 캐릭터를 삭제하면 플레이어를 비활성화.
        JY_AvatarLoad.s_instance.charMale.gameObject.SetActive(false);
        JY_AvatarLoad.s_instance.charFemale.gameObject.SetActive(false);
        JY_CharacterListManager.s_instance.DeleteCharacter(_num);
        JY_CharacterListManager.s_instance.selectNum = -1;
        PanelSwitch();
    }

    private void SelectCharacter(int _num)
    {
        //선택 취소 구현
        if (JY_CharacterListManager.s_instance.selectNum == _num)
        {
            JY_CharacterListManager.s_instance.selectNum = -1;
            JY_AvatarLoad.s_instance.charMale.gameObject.SetActive(false);
            JY_AvatarLoad.s_instance.charFemale.gameObject.SetActive(false);            
            PanelSwitch();
            return;
        }
        JY_CharacterListManager.s_instance.selectNum = _num;            
        JY_AvatarLoad.s_instance.LoadModelData(_num);        // 선택한 성별 캐릭터 활성화. inventory onenable
        // 선택한 캐릭터의 인벤토리를 카피해옴.
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
