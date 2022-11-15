using System.Collections;
using System.Collections.Generic;
using CartoonHeroes;
using static CartoonHeroes.SetCharacter;
using UnityEngine;

// �κ� �������� ����.
public class JY_AvatarLoad : MonoBehaviour
{
    #region �̱��� ����. �ܺο��� s_instance ���.
    private static JY_AvatarLoad instance;
    public static JY_AvatarLoad s_instance { get { return instance; } }
    #endregion    
        
    public Transform origin;               // Player
    public GameObject charMale;             // ����
    public GameObject charFemale;           // ����
    public GameObject charWeaponDummy;
    public GameObject charShieldDummy;    
    public SetCharacter setChara;
    public Inventory inven;
    
    void Awake()
    {
        // ���� ������Ʈ�� ����ϴ� ĳ���͸���Ʈ�Ŵ������� �̱����� ��� ���̹Ƿ� DontDestroyOnLoad�� �� �ʿ� ����.
        instance = this;
        inven = GetComponent<Inventory>();
    }

    void Start()
    {
        origin = this.transform;
        charMale = FindGameObjectInChild("BaseCharacterM", origin).gameObject;
        charFemale = FindGameObjectInChild("BaseCharacterF", origin).gameObject;

        // �κ���� ���۵� �� CharacterListManager�� selectNum�� -1�� �ʱ�ȭ����.
        JY_CharacterListManager.s_instance.selectNum = -1;
    }

    public Transform FindGameObjectInChild(string nodename, Transform origin)
    {        
        if (origin.name == nodename)
            return origin;
        for (int i = 0; i < origin.childCount; i++)
        {
            Transform childTr = FindGameObjectInChild(nodename, origin.GetChild(i));
            if (childTr != null)
                return childTr;
        }
        return null;
    }

    public void SubOptionLoad(int currentOption, int sub)
    {
        // ������ �����ϴ� �������� ������.
        for (int i = 0; i < setChara.itemGroups[currentOption].slots; i++)
        {
            if (setChara.HasItem(setChara.itemGroups[currentOption], i))
            {
                setChara.GetRemoveObjList(setChara.itemGroups[currentOption], i).ForEach(e =>
                {
                    if (e != null)
                    {
                        DestroyImmediate(e);
                    }
                });
            }
        }
        setChara.AddItem(setChara.itemGroups[currentOption], sub);        
    }

    public void LoadModelData(int listNum)
    {
        //gender ����
        switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender)
        {
            case EGender.FEMALE:
                {
                    charMale.SetActive(false);
                    charFemale.SetActive(true);
                    setChara = charFemale.GetComponent<SetCharacter>();
                }
                break;
            case EGender.MALE:
                {
                    charFemale.SetActive(false);
                    charMale.SetActive(true);
                    setChara = charMale.GetComponent<SetCharacter>();
                }
                break;
        }
        for (int i = 0; i < 4; i++)
        {
            SubOptionLoad(i, JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].characterAvatar[i]);
        }
        LobbyDummyClear(listNum);
    }



    // ���� ��� �ִ� ����� ���и� ������.
    public void LobbyDummyClear(int listNum)
    {
        if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender.Equals(EGender.MALE))
        {
            charWeaponDummy = FindGameObjectInChild("Character R Weapon Slot", charMale.transform).gameObject;
            charShieldDummy = FindGameObjectInChild("Character L Weapon Slot", charMale.transform).gameObject;
        }
        else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender.Equals(EGender.FEMALE))
        {
            charWeaponDummy = FindGameObjectInChild("Character R Weapon Slot", charFemale.transform).gameObject;
            charShieldDummy = FindGameObjectInChild("Character L Weapon Slot", charFemale.transform).gameObject;
        }

        for (int i = 0; i < charWeaponDummy.transform.childCount; i++)
            Destroy(charWeaponDummy.transform.GetChild(i).gameObject);
        for (int i = 0; i < charShieldDummy.transform.childCount; i++)
            Destroy(charShieldDummy.transform.GetChild(i).gameObject);
    }
}
