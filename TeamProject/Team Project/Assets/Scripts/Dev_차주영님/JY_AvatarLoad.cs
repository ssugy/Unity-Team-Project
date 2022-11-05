using System.Collections;
using System.Collections.Generic;
using CartoonHeroes;
using static CartoonHeroes.SetCharacter;
using UnityEngine;

public class JY_AvatarLoad : MonoBehaviour
{
    #region 싱글톤 패턴. 외부에선 s_instance 사용.
    private static JY_AvatarLoad instance;
    public static JY_AvatarLoad s_instance { get { return instance; } }
    #endregion    
        
    public Transform origin;               // Player
    public GameObject charMale;             // 남자
    public GameObject charFemale;           // 여자
    public GameObject charWeaponDummy;
    public GameObject charShieldDummy;
    public GameObject charWeapon;
    public GameObject charShield;
    public SetCharacter setChara;
    
    void Awake()
    {
        // 같은 오브젝트를 사용하는 캐릭터리스트매니저에서 싱글톤을 사용 중이므로 DontDestroyOnLoad를 할 필요 없음.
        instance ??= this;
    }

    void Start()
    {
        origin = JY_PlayerReturn.instance.GetPlayerOrigin();
        charMale = FindGameObjectInChild("BaseCharacterM", origin).gameObject;
        charFemale = FindGameObjectInChild("BaseCharacterF", origin).gameObject;
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

    void DeleteSubOption(int currentOption)
    {
        for (int j = 0; j < setChara.itemGroups[currentOption].slots; j++)
        {
            if (setChara.HasItem(setChara.itemGroups[currentOption], j))
            {
                List<GameObject> removedObjs = setChara.GetRemoveObjList(setChara.itemGroups[currentOption], j);
                for (int m = 0; m < removedObjs.Count; m++)
                {
                    if (removedObjs[m] != null)
                    {
                        DestroyImmediate(removedObjs[m]);
                    }
                }
            }
        }

    }
    public void subOptionLoad(int currentOption, int sub)
    {
        DeleteSubOption(currentOption);
        {
            GameObject addedObj = setChara.AddItem(setChara.itemGroups[currentOption], sub);
        }
    }

    public void LoadModelData(int listNum)
    {
        //LobbyDummyClear(listNum);
        if (JY_CharacterListManager.s_instance.selectNum == -1)
        {            
            charMale.SetActive(false);
            charFemale.SetActive(false);
            charWeaponDummy = null;
            charShieldDummy = null;
        }
        else
        {
            //gender 갱신
            if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender.Equals(EGender.FEMALE))
            {
                charMale.SetActive(false);
                charFemale.SetActive(true);
                setChara = charFemale.GetComponent<SetCharacter>();
            }
            else
            {
                charFemale.SetActive(false);
                charMale.SetActive(true);
                setChara = charMale.GetComponent<SetCharacter>();
            }
            for (int i = 0; i < 4; i++)
            {
                subOptionLoad(i, JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].characterAvatar[i]);
            }
        }
    }

    public void equipWeapon(int listNum)
    {
        if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender.Equals(EGender.MALE))
        {
            charWeaponDummy = FindGameObjectInChild("Character R Weapon Slot", charMale.transform).gameObject;            
        }
        else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender.Equals(EGender.FEMALE))
        {
            charWeaponDummy = FindGameObjectInChild("Character R Weapon Slot", charFemale.transform).gameObject;            
        }
        GameObject weaponSc = Resources.Load<GameObject>("Item/Weapon/Sword_1");
        charWeapon ??= Instantiate<GameObject>(weaponSc);        
        charWeapon.transform.SetParent(charWeaponDummy.transform);
        charWeapon.transform.localPosition = Vector3.zero;
        charWeapon.transform.localRotation = Quaternion.identity;


    }


    // 원래 들고 있는 무기와 방패를 삭제함.
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
