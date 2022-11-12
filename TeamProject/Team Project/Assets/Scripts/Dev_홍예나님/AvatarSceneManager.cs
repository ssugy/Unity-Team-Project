using CartoonHeroes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using static CartoonHeroes.SetCharacter;
using static GameManager;
//using Unity.VisualScripting;


public class AvatarSceneManager : MonoBehaviour
{
    private enum Steps { SELECT_JOB, SELECT_BODY, SELECT_NAME, SAVE }
    private enum MoreOptions { FACE, HAIR, TORSO, LEGS, LAST }
    
    const int MIN_NAME_LENGTH = 2;

    // �ʱ� ����Ʈ ����.
    private Steps currentStep = Steps.SELECT_JOB;
    private MoreOptions currentOption = MoreOptions.FACE;
    private int currentOptionPanel = 0;


    public GameObject zoomSlider;
    public GameObject popupToLobby;
    public GameObject popupNameIssue;
    public List<GameObject> canvases;
    public List<GameObject> options;
    public List<Slider> sliders;
    public GameObject charMale;
    public GameObject charFemale;
    public Transform mRWeapon;
    public Transform mLWeapon;
    public Transform fRWeapon;
    public Transform fLWeapon;

    public GameObject warriorInfo;
    public GameObject magicianInfo;


    private EGender gender;
    private EJob job;
    private List<int> optionSubs;
    public InputField CharacterNameInput;
    private SetCharacter targetScript;
    private Dictionary<string, int> mapOptionNames;

    private string[] strOptionNames = { "Heads", "Hairs", "Torsos", "Legs" };
    private int[] numBlendOptions = { 6, 2, 4, 1 };

    // �� Item Group���� Option(n��)���� �����ϱ� ���� ������ �ʿ��մϴ�
    private List<float> currentBlendOption;
    private List<float> faceOptions = new List<float>();     // head�� �ɼǵ�
    private List<float> hairOptions = new List<float>();     // hair�� �ɼǵ�    
    private List<float> torsoOptions = new List<float>();    // torso�� �ɼǵ�
    private List<float> legOptions = new List<float>();      // Leg�� �ɼǵ�
    private SkinnedMeshRenderer currentSkinnedMesh = null;

    // ���� �������� ��, Ÿ�� �������� �������.
    private void OnDisable() => Time.timeScale = 1f;    
    void Start()
    {
#if UNITY_EDITOR

#else
        // ����� ���忡���� zoomslider�� �����
        
        zoomSlider.SetActive(false);        
#endif        
        ShowCanvas();

        // 1. Item group(4��) ���� MapOptionNames Dictionary�� �ʱ�ȭ �մϴ�. 
        optionSubs = new List<int>();
        for (int i = 0; i < (int)MoreOptions.LAST; i++)
        {
            optionSubs.Add(0);
        }
        mapOptionNames = new Dictionary<string, int>();
        // 2. Item Group ���� setting������ blendOption ��(n��)�� ������ �ʱ�ȭ �մϴ�.
        for (int i = 0; i < (int)MoreOptions.LAST; i++)
        {
            List<float> blendOption;
            switch ((MoreOptions)i)
            {
                case MoreOptions.FACE:
                    blendOption = faceOptions;
                    break;
                case MoreOptions.HAIR:
                    blendOption = hairOptions;
                    break;
                case MoreOptions.TORSO:
                    blendOption = torsoOptions;
                    break;
                default:
                    blendOption = legOptions;
                    break;
            }
            for (int j = 0; j < numBlendOptions[i]; j++)
            {
                blendOption.Add(0);
            }
        }

        gender = EGender.MALE;
        job = EJob.WARRIOR;
        ShowCharacter();
        //ù��° ��ư�� ���������� ������
        ShowOption(0);

    }

    // key-value �˻��� ���� Dictionary�� �ʱ�ȭ, �� option ����� �����.
    // key �� strOptionNames �� 4���� �̸��̴�.
    // value�� targetscript(SetCharacter.cs)���� �����ϴ� ItemGroup �̸��̴�.
    private void SetMap()
    {
        mapOptionNames.Clear();
        int i = 0;
        // �� Itemgroup�� �̸��� �����ͼ� Dictionary�� ä���ִ´�.
        // Key �� �̸��̰�, Value�� �� �̸��� �����ϰ� �ִ� �����̴�.
        // ��, Legs�� ���� ���� ����ִٸ� Legs�� ������ 0�� �ȴ�.
        Array.ForEach(targetScript.itemGroups, e => mapOptionNames.Add(e.name, i++));
    }

    private void ShowCharacter()
    {
        switch (gender)
        {
            case EGender.MALE:
                {
                    charMale.SetActive(true);
                    charFemale.SetActive(false);
                    targetScript = charMale.GetComponent<SetCharacter>();
                }
                break;
            case EGender.FEMALE:
                {
                    charMale.SetActive(false);
                    charFemale.SetActive(true);
                    targetScript = charFemale.GetComponent<SetCharacter>();
                }
                break;
            default:
                {
                    charMale.SetActive(false);
                    charFemale.SetActive(false);
                    targetScript = null;
                }
                break;
        }        
        SetMap();
    }

    private void HideAllCanvases() => canvases.ForEach(e => e.SetActive(false));

    private void ShowCanvas()
    {
        HideAllCanvases();
        if (currentStep >= Steps.SELECT_JOB && currentStep <= Steps.SELECT_NAME) 
            canvases[(int)currentStep].SetActive(true);
    }       

    public void OnClickBackToLobby() => GameManager.s_instance.LoadScene((int)SceneName.Lobby);

    public void OnClickPopup() => popupToLobby.SetActive(popupToLobby.activeSelf ? false : true);        

    public void OnClickPause() => Time.timeScale = (Time.timeScale > 0f ? 0f : 1f);    

    public void OnClickNext()
    {
        if(currentStep < Steps.SELECT_NAME)
        {
            currentStep++;            
            ShowCanvas();
        }
        // �̸� ���ڼ��� �ּ� ���ں��� ������ return.
        else if (currentStep == Steps.SELECT_NAME && CharacterNameInput.text.Length < MIN_NAME_LENGTH)
        {
            popupNameIssue.SetActive(true);
            return;
        }
        else
        {            
            if (JY_CharacterListManager.s_instance != null)
            {
                //�󽽷� ã�� �� �ۼ� �� break
                for (int i = 0; i < 4; i++)
                {
                    if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].isNull == true)
                    {
                        if (CharacterNameInput!=null)
                        {
                            //�������ͽ� �ۼ�
                            InfoData tmp = JY_CharacterListManager.s_instance.jInfoData.infoDataList[i];
                            tmp.name = CharacterNameInput.text;
                            tmp.isNull = false;
                            tmp.level = 1;
                            tmp.job = job;                            
                            tmp.gender = gender;     
                            
                            //�𵨸� �ۼ�
                            tmp.characterAvatar[0] = optionSubs[0];
                            tmp.characterAvatar[1] = optionSubs[1];
                            tmp.characterAvatar[2] = optionSubs[2];
                            tmp.characterAvatar[3] = optionSubs[3];

                            // ������ �ۼ�                            
                            switch (job)
                            {
                                case EJob.WARRIOR:
                                    {
                                        tmp.status = new int[4] { 7, 6, 10, 5 };
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "�ռҵ�", 1));
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "�κ��� ����", 1));
                                        break;
                                    }
                                case EJob.MAGICIAN:
                                    {
                                        tmp.status = new int[4] { 5, 9, 5, 9 };
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "�ܵ�", 1));
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "��� ������", 1));
                                        break;
                                    }
                            }           // ����
                            switch (optionSubs[1]) 
                            {
                                case 1:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "ö�� ����", 1));
                                        break;
                                    }
                                case 2:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "������ ����", 1));
                                        break;
                                    }
                                case 3:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "���� ����", 1));
                                        break;
                                    }
                                default: break;
                            } // ����
                            switch (optionSubs[0]) 
                            {
                                case 1:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "ö�� �ٸ� ��ȣ��", 1));
                                        break;
                                    }
                                case 2:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "�氩 ����", 1));
                                        break;
                                    }
                                case 3:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "���� ����", 1));
                                        break;
                                    }
                                default: break;
                            } // ����
                            JY_CharacterListManager.s_instance.jInfoData.infoDataList[i] = tmp;
                            break;
                        }
                    }
                    // ���� �̸��� ������ ����.
                    else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].name.Equals(CharacterNameInput.text))
                    {
                        popupNameIssue.SetActive(true);
                        return;
                    }
                }

                if (GameManager.s_instance != null)
                    GameManager.s_instance.LoadScene((int)SceneName.Lobby);
            }
        }
    }

    public void OnClickPrev()
    {
        if (currentStep > Steps.SELECT_JOB)
        {
            currentStep--;           
            ShowCanvas();
        }
    }

    public void OnClickFemale()
    {
        gender = EGender.FEMALE;
        ShowCharacter();
        ShowOption(0);
        sliders.ForEach(e => { if (e.transform.parent.parent.parent.parent.gameObject.activeSelf) e.value -= 0.01f; });
    }

    public void OnClickMale()
    {
        gender = EGender.MALE;
        ShowCharacter();
        ShowOption(0);
        sliders.ForEach(e => { if (e.transform.parent.parent.parent.parent.gameObject.activeSelf) e.value -= 0.01f; });
    }

    public void OnClickWarrior()
    {
        job = EJob.WARRIOR;
        warriorInfo.SetActive(true);
        magicianInfo.SetActive(false);
        for (int i = 0; i < fRWeapon.childCount; i++)
            Destroy(fRWeapon.GetChild(i).gameObject);
        for (int i = 0; i < fLWeapon.childCount; i++)
            Destroy(fLWeapon.GetChild(i).gameObject);
        for (int i = 0; i < mRWeapon.childCount; i++)
            Destroy(mRWeapon.GetChild(i).gameObject);
        for (int i = 0; i < mLWeapon.childCount; i++)
            Destroy(mLWeapon.GetChild(i).gameObject);
        GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/Sword_1");
        GameObject shieldSrc = Resources.Load<GameObject>("Item/Shield/Shield_2");
        if (weaponSrc != null)
            Instantiate(weaponSrc, fRWeapon);
        if (shieldSrc != null)
            Instantiate(shieldSrc, fLWeapon);               
        if (weaponSrc != null)
            Instantiate(weaponSrc, mRWeapon);
        if (shieldSrc != null)
            Instantiate(shieldSrc, mLWeapon);             
    }
    public void OnClickMagician()
    {
        job = EJob.MAGICIAN;
        warriorInfo.SetActive(false);
        magicianInfo.SetActive(true);
        for (int i = 0; i < fRWeapon.childCount; i++)
            Destroy(fRWeapon.GetChild(i).gameObject);
        for (int i = 0; i < fLWeapon.childCount; i++)
            Destroy(fLWeapon.GetChild(i).gameObject);
        for (int i = 0; i < mRWeapon.childCount; i++)
            Destroy(mRWeapon.GetChild(i).gameObject);
        for (int i = 0; i < mLWeapon.childCount; i++)
            Destroy(mLWeapon.GetChild(i).gameObject);
        GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/Dagger_2");
        GameObject shieldSrc = Resources.Load<GameObject>("Item/Shield/Staff_2");
        if (weaponSrc != null)
            Instantiate(weaponSrc, fRWeapon);
        if (shieldSrc != null)
            Instantiate(shieldSrc, fLWeapon);                
        if (weaponSrc != null)
            Instantiate(weaponSrc, mRWeapon);
        if (shieldSrc != null)
            Instantiate(shieldSrc, mLWeapon);
    }

    private void ShowOption(int option)
    {
        HideAllOption();
        // 1. strOptionNames ���� Item Group�� Name�� ����ִ�.
        //    �츮�� ���ϴ� Group�� �̸� �� option��ư�� ������ �̹� �˰��ִ�. (Face = 0, Hair, Torso, Leg = 3)
        //    �� �̸��� strOptionNames�� ������� ����ִ�.
        // 2. mapOptionNames�� key-value �˻��� ���� Dictionary�̴�.
        //    mapOptionNames�� �̹� SetMap���� �ʱ�ȭ �Ǿ� �ִ�.
        // 3. int option�� ���� ���� ��ư�� �����̴�.
        //    strOptionNames[option]�� ���� ���� ��ư�� �����ϴ� Group�� �̸��̴�.
        //    mapOptionNames[strOptionNames[option]]�� ���� ���� ��ư�� group�̸��� ���� ����� Item Group�� ���� Index�̴�.
        //    �װ��� currentOption�̴�.
        // 4. current Option���� ���� targetScript.itemGroups[ current Option ] �̷������� ���ȴ�.
        currentOption = (MoreOptions)mapOptionNames[strOptionNames[option]];
        options[currentOptionPanel].SetActive(true);
        switch ((MoreOptions)option)
        {
            case MoreOptions.FACE:
                currentBlendOption = faceOptions;
                break;
            case MoreOptions.HAIR:
                currentBlendOption = hairOptions;
                break;
            case MoreOptions.TORSO:
                currentBlendOption = torsoOptions;
                break;
            case MoreOptions.LEGS:
                currentBlendOption = legOptions;
                break;
        }
        currentSkinnedMesh = GetSkinnedMesh();        
    }

    public void HideAllOption() => options.ForEach(e => e.SetActive(false));    

    public void OnClickOption(int option)
    {
        currentOptionPanel = option;
        if (mapOptionNames.ContainsKey(strOptionNames[option]))        
            ShowOption(option);
        // �� A���� �� B�� �����ϴ� ��, �ɼ��� ������ �� �����̴��� �̺�Ʈ�� ȣ���ϱ� ����.
        // ���� �ٲ��� ������ �̺�Ʈ�� ȣ����� ����.
        sliders.ForEach(e => { if (e.transform.parent.parent.parent.parent.gameObject.activeSelf) e.value -= 0.01f; });
    }

    // head1�� hair2���� subOption ���ù�ư�� �������� ������ �����ϴ� head �Ǵ� hair�� �����Ѵ�.
    private void DeleteSubOption()
    {
        int i = (int)currentOption;
        for (int j = 0; j < targetScript.itemGroups[i].slots; j++)
        {
            if (targetScript.HasItem(targetScript.itemGroups[i], j))
            {
                List<GameObject> removedObjs = targetScript.GetRemoveObjList(targetScript.itemGroups[i], j);
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

    // ���� �ִ� �𵨿��� �־��� option�� �ش��ϴ� child Object�� ã�Ƴ���.
    // ���� ���, ���� currentOption�� hair��� hair�� �ش�Ǵ� object�� ã�Ƴ���.
    private SkinnedMeshRenderer GetSkinnedMesh()
    {
        Transform[] allChildren = targetScript.GetAllCharacterChildren();

        //List<GameObject> selectedList = new List<GameObject>();

        for (int i = 0; i < allChildren.Length; i++)
        {
            for (int j = 0; j < targetScript.itemGroups[(int)currentOption].slots; j++)
            if (targetScript.BelongsToItem(allChildren[i], targetScript.itemGroups[(int)currentOption], j))
            {
                //selectedList.Add(allChildren[i].gameObject);
                GameObject candidate = allChildren[i].gameObject;
                if (candidate != null)
                {
                    if (candidate.GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        return candidate.GetComponent<SkinnedMeshRenderer>();
                    }
                    if (candidate.transform.childCount > 0)
                    {
                        candidate = candidate.transform.GetChild(0).gameObject;
                        if (candidate.GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            return candidate.GetComponent<SkinnedMeshRenderer>();
                        }
                    }

                }
            }
        }
        return null;
    }

    public void OnClickSubOption(int sub)
    {
        int i = (int)currentOption;
        DeleteSubOption();
        {
            optionSubs[i] = sub;
            GameObject addedObj = targetScript.AddItem(targetScript.itemGroups[i], optionSubs[i]);
            currentSkinnedMesh = GetSkinnedMesh();
        }
        sliders.ForEach(e => { if (e.transform.parent.parent.parent.parent.gameObject.activeSelf) e.value -= 0.01f; });
    }
    private void OnChangeSlide(int index, float val)
    {
        if (index >= currentBlendOption.Count)
        {
            return;
        }
        currentBlendOption[index] = val;
        if (currentSkinnedMesh != null)
        {
            if (currentSkinnedMesh.sharedMesh.blendShapeCount > index)
            {
                currentSkinnedMesh.SetBlendShapeWeight(index, val);
            }
        }
    }

    public void OnChangeSlide1(float val) => OnChangeSlide(0, val);    

    public void OnChangeSlide2(float val) => OnChangeSlide(1, val);

    public void OnChangeSlide3(float val) => OnChangeSlide(2, val);

    public void OnChangeSlide4(float val) => OnChangeSlide(3, val);

    public void OnChangeSlide5(float val) => OnChangeSlide(4, val);

    public void OnChangeSlide6(float val) => OnChangeSlide(5, val);    
}
