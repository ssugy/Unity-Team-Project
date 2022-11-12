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

    // 초기 셀렉트 상태.
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

    // 각 Item Group별로 Option(n개)값을 저장하기 위한 변수가 필요합니다
    private List<float> currentBlendOption;
    private List<float> faceOptions = new List<float>();     // head용 옵션들
    private List<float> hairOptions = new List<float>();     // hair용 옵션들    
    private List<float> torsoOptions = new List<float>();    // torso용 옵션들
    private List<float> legOptions = new List<float>();      // Leg용 옵션들
    private SkinnedMeshRenderer currentSkinnedMesh = null;

    // 씬을 빠져나갈 때, 타임 스케일을 원래대로.
    private void OnDisable() => Time.timeScale = 1f;    
    void Start()
    {
#if UNITY_EDITOR

#else
        // 모바일 빌드에서만 zoomslider가 사라짐
        
        zoomSlider.SetActive(false);        
#endif        
        ShowCanvas();

        // 1. Item group(4개) 별로 MapOptionNames Dictionary를 초기화 합니다. 
        optionSubs = new List<int>();
        for (int i = 0; i < (int)MoreOptions.LAST; i++)
        {
            optionSubs.Add(0);
        }
        mapOptionNames = new Dictionary<string, int>();
        // 2. Item Group 별로 setting가능한 blendOption 값(n개)의 갯수를 초기화 합니다.
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
        //첫번째 버튼을 누른것으로 간주함
        ShowOption(0);

    }

    // key-value 검색을 위한 Dictionary의 초기화, 즉 option 목록을 만든다.
    // key 는 strOptionNames 의 4가지 이름이다.
    // value는 targetscript(SetCharacter.cs)에서 제공하는 ItemGroup 이름이다.
    private void SetMap()
    {
        mapOptionNames.Clear();
        int i = 0;
        // 각 Itemgroup의 이름을 가져와서 Dictionary에 채워넣는다.
        // Key 는 이름이고, Value는 그 이름이 차지하고 있는 순서이다.
        // 즉, Legs가 가장 먼저 들어있다면 Legs의 순서는 0이 된다.
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
        // 이름 글자수가 최소 글자보다 작으면 return.
        else if (currentStep == Steps.SELECT_NAME && CharacterNameInput.text.Length < MIN_NAME_LENGTH)
        {
            popupNameIssue.SetActive(true);
            return;
        }
        else
        {            
            if (JY_CharacterListManager.s_instance != null)
            {
                //빈슬롯 찾은 후 작성 및 break
                for (int i = 0; i < 4; i++)
                {
                    if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[i].isNull == true)
                    {
                        if (CharacterNameInput!=null)
                        {
                            //스테이터스 작성
                            InfoData tmp = JY_CharacterListManager.s_instance.jInfoData.infoDataList[i];
                            tmp.name = CharacterNameInput.text;
                            tmp.isNull = false;
                            tmp.level = 1;
                            tmp.job = job;                            
                            tmp.gender = gender;     
                            
                            //모델링 작성
                            tmp.characterAvatar[0] = optionSubs[0];
                            tmp.characterAvatar[1] = optionSubs[1];
                            tmp.characterAvatar[2] = optionSubs[2];
                            tmp.characterAvatar[3] = optionSubs[3];

                            // 아이템 작성                            
                            switch (job)
                            {
                                case EJob.WARRIOR:
                                    {
                                        tmp.status = new int[4] { 7, 6, 10, 5 };
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "롱소드", 1));
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "널빤지 방패", 1));
                                        break;
                                    }
                                case EJob.MAGICIAN:
                                    {
                                        tmp.status = new int[4] { 5, 9, 5, 9 };
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "단도", 1));
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "우든 스태프", 1));
                                        break;
                                    }
                            }           // 무기
                            switch (optionSubs[1]) 
                            {
                                case 1:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "철제 갑옷", 1));
                                        break;
                                    }
                                case 2:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "가벼운 갑옷", 1));
                                        break;
                                    }
                                case 3:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "가죽 상의", 1));
                                        break;
                                    }
                                default: break;
                            } // 상의
                            switch (optionSubs[0]) 
                            {
                                case 1:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "철제 다리 보호대", 1));
                                        break;
                                    }
                                case 2:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "경갑 하의", 1));
                                        break;
                                    }
                                case 3:
                                    {
                                        tmp.itemList.Add(new(ItemType.EQUIPMENT, EquipState.EQUIPED, "가죽 하의", 1));
                                        break;
                                    }
                                default: break;
                            } // 하의
                            JY_CharacterListManager.s_instance.jInfoData.infoDataList[i] = tmp;
                            break;
                        }
                    }
                    // 같은 이름이 있으면 리턴.
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
        // 1. strOptionNames 에는 Item Group의 Name이 들어있다.
        //    우리가 원하는 Group의 이름 및 option버튼의 순서는 이미 알고있다. (Face = 0, Hair, Torso, Leg = 3)
        //    이 이름은 strOptionNames에 순서대로 들어있다.
        // 2. mapOptionNames는 key-value 검색을 위한 Dictionary이다.
        //    mapOptionNames는 이미 SetMap으로 초기화 되어 있다.
        // 3. int option은 내가 누른 버튼의 순서이다.
        //    strOptionNames[option]은 내가 누른 버튼이 지정하는 Group의 이름이다.
        //    mapOptionNames[strOptionNames[option]]은 내가 누른 버튼의 group이름을 통해 얻어진 Item Group의 실제 Index이다.
        //    그것이 currentOption이다.
        // 4. current Option값은 추후 targetScript.itemGroups[ current Option ] 이런식으로 사용된다.
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
        // 얼굴 A에서 얼굴 B로 변경하는 등, 옵션을 변경할 때 슬라이더의 이벤트를 호출하기 위함.
        // 값이 바뀌지 않으면 이벤트가 호출되지 않음.
        sliders.ForEach(e => { if (e.transform.parent.parent.parent.parent.gameObject.activeSelf) e.value -= 0.01f; });
    }

    // head1나 hair2등의 subOption 선택버튼을 눌렀을때 기존에 존재하는 head 또는 hair를 삭제한다.
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

    // 현재 있는 모델에서 주어진 option에 해당하는 child Object를 찾아낸다.
    // 예를 들어, 현재 currentOption이 hair라면 hair에 해당되는 object를 찾아낸다.
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
