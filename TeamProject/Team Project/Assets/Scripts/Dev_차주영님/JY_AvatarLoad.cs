using System.Collections;
using System.Collections.Generic;
using CartoonHeroes;
using static CartoonHeroes.SetCharacter;
using UnityEngine;

public class JY_AvatarLoad : MonoBehaviour
{
    public static JY_AvatarLoad instance;
    public static JY_AvatarLoad s_instance { get { return instance; } }
    public GameObject origin;

    public GameObject charMale;
    public GameObject charFemale;
    public GameObject charWeaponDummy;
    public GameObject charShieldDummy;
    public GameObject charWeapon;
    public GameObject charShield;

    public SetCharacter setChara;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        origin = JY_PlayerReturn.instance.getPlayerOrigin();
        charMale = findGameObjectInChild("BaseCharacterM", origin.transform).gameObject;
        charFemale = findGameObjectInChild("BaseCharacterF", origin.transform).gameObject;
    }

    public Transform findGameObjectInChild(string nodename, Transform origin)
    {
        if (origin.name == nodename)
            return origin;
        for (int i = 0; i < origin.childCount; i++)
        {
            Transform childTr = findGameObjectInChild(nodename, origin.GetChild(i));
            if (childTr != null)
                return childTr;
        }
        return null;
    }


    // Update is called once per frame
    void Update()
    {

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
            //gender °»½Å
            if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender == "F")
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
        if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender == "M")
        {
            charWeaponDummy = findGameObjectInChild("Character R Weapon Slot", charMale.transform).gameObject;            
        }
        else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender == "F")
        {
            charWeaponDummy = findGameObjectInChild("Character R Weapon Slot", charFemale.transform).gameObject;            
        }
        GameObject weaponSc = Resources.Load<GameObject>("Item/Weapon/Sword_1");
        if (charWeapon == null)
        {
            charWeapon = GameObject.Instantiate<GameObject>(weaponSc);
        }
        charWeapon.transform.SetParent(charWeaponDummy.transform);
        charWeapon.transform.localPosition = Vector3.zero;
        charWeapon.transform.localRotation = Quaternion.identity;


    }

    public void LobbyDummyClear(int listNum)
    {
        if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender == "M")
        {
            charWeaponDummy = findGameObjectInChild("Character R Weapon Slot", charMale.transform).gameObject;
            charShieldDummy = findGameObjectInChild("Character L Weapon Slot", charMale.transform).gameObject;
        }
        else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[listNum].gender == "F")
        {
            charWeaponDummy = findGameObjectInChild("Character R Weapon Slot", charFemale.transform).gameObject;
            charShieldDummy = findGameObjectInChild("Character L Weapon Slot", charFemale.transform).gameObject;
        }

        for (int i = 0; i < charWeaponDummy.transform.childCount; i++)
            Destroy(charWeaponDummy.transform.GetChild(i).gameObject);
        for (int i = 0; i < charShieldDummy.transform.childCount; i++)
            Destroy(charShieldDummy.transform.GetChild(i).gameObject);
    }
}
