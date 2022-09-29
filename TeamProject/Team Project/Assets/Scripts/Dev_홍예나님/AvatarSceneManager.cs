using CartoonHeroes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CartoonHeroes.SetCharacter;
using static GameManager;
using Unity.VisualScripting;

public class AvatarSceneManager : MonoBehaviour
{
    private enum Steps{ SELECT_JOB, SELECT_BODY, SELECT_NAME, SAVE}
    private enum MoreOptions { FACE, HAIR, TORSO, LEGS, LAST}
    public enum Gender { MALE, FEMALE, NEUTRAL}
    const float WEIGHT_SCALE = 100.0f;
    private Steps currentStep = Steps.SELECT_JOB;
    private MoreOptions currentOption = MoreOptions.FACE;
    private int currentOptionPanel = 0;


    public GameObject popup;
    public List<GameObject> canvases;
    public List<GameObject> options;
    public GameObject charMale;
    public GameObject charFemale;
    private Gender gender;
    private List<int> optionSubs;
    public InputField CharacterNameInput;
    private SetCharacter targetScript;
    private Dictionary<string, int> mapOptionNames;

    private string[] strOptionNames = {"Heads", "Hairs", "Torsos", "Legs" };
    private int[] numBlendOptions = { 6, 2, 4, 1 };

    // 각 Item Group별로 Option(n개)값을 저장하기 위한 변수가 필요합니다
    private List<float> currentBlendOption;
    private List<float> legOptions = new List<float>(); // Leg용 옵션들
    private List<float> hairOptions = new List<float>(); // hair용 옵션들
    private List<float> headOptions = new List<float>(); // head용 옵션들
    private List<float> torsoOptions = new List<float>(); // torso용 옵션들
    private SkinnedMeshRenderer currentSkinnedMesh = null;

    // Start is called before the first frame update
    void Start()
    {
        ShowCanvas();

        // 1. Item group(4개) 별로 MapOptionNames Dictionary를 초기화 합니다. 
        optionSubs = new List<int>();
        for(int i = 0; i < (int)MoreOptions.LAST; i++)
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
                    blendOption = headOptions;
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

        gender = Gender.MALE;
        ShowCharacter();
        //첫번째 버튼을 누른것으로 간주함
        ShowOption(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        foreach (var itemGroup in targetScript.itemGroups)
        {
            mapOptionNames.Add(itemGroup.name, i);
            i++;
        }
    }

    private void ShowCharacter()
    {
        if(gender == Gender.FEMALE)
        {
            charMale.SetActive(false);
            charFemale.SetActive(true);
            targetScript = charFemale.GetComponent<SetCharacter>();
        }
        else
        {
            charMale.SetActive(true);
            charFemale.SetActive(false);
            targetScript = charMale.GetComponent<SetCharacter>();
        }
        SetMap();
    }

    private void HideAllCanvases()
    {
        foreach (var canvas in canvases)
        {
            canvas.SetActive(false);
        }
    }

    private void ShowCanvas()
    {
        if(currentStep >= Steps.SELECT_JOB && currentStep <= Steps.SELECT_NAME)
        {
            HideAllCanvases();
            canvases[(int)currentStep].SetActive(true);
        }
    }

    public void OnClickBackToLobby()
    {
        LoadingSceneController.LoadScene((int)SceneName.Robby);
    }

    public void OnClickClosePopup()
    {
        popup.SetActive(false);
    }

    public void OnClickOpenPopup()
    {
        popup.SetActive(true);
    }

    public void OnClickPauseAvatar()
    {
        if(Time.timeScale > 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void OnClickNext()
    {
        if(currentStep < Steps.SELECT_NAME)
        {
            currentStep++;
            ShowCanvas();
        }

        else
        {
            //주영
            //캐릭터 생성 버튼을 누른 경우 로비씬으로 데이터를 넘깁니다.
            //로비씬을 경유하여 캐릭터메이킹씬을 시작했을 때에만 로비씬으로 데이터를 넘깁니다.
            //상황에 따라 추후에 게임매니저와 병합하는 작업이 필요할 것 같습니다.
            if (JY_CharacterListManager.s_instance != null)
            {
                //빈슬롯 찾은 후 작성 및 break
                for (int i = 0; i < 4; i++)
                {
                    if (JY_CharacterListManager.s_instance.characterData.infoDataList[i].isNull == true)
                    {
                        if (CharacterNameInput!=null)
                        {
                            //스테이터스 작성
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].name = CharacterNameInput.text;
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].isNull = false;
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].level = 1;
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].job = "전사";
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].gender = (gender == 0 ? "M" : "F");
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].species = "인간";
                            //모델링 작성
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[0] = optionSubs[0];
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[1] = optionSubs[1];
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[2] = optionSubs[2];
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[3] = optionSubs[3];
                            break;
                        }
                    }
                }
                //데이터 세이브
                JY_CharacterListManager.s_instance.saveListData();
                if (GameManager.s_instance != null)
                    GameManager.s_instance.LoadScene(2);
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
        gender = Gender.FEMALE;
        ShowCharacter();
        ShowOption(0);
    }

    public void OnClickMale()
    {
        gender = Gender.MALE;
        ShowCharacter();
        ShowOption(0);
    }

    private void ShowOption(int option)
    {
        HideAllOption();
        // 1. strOptionNames 에는 Item Group의 Name이 들어있다.
        //    우리가 원하는 Group의 이름 및 option버튼의 순서는 이미 알고있다. (Hairs = 0, Heads, Torsos, Legs = 3)
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
                currentBlendOption = headOptions;
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
        if (currentSkinnedMesh != null)
        {
            Debug.Log("Found");
        }

    }

    public void HideAllOption()
    {
        foreach (var option in options)
        {
            option.SetActive(false);
        }
    }

    public void OnClickOption(int option)
    {
        currentOptionPanel = option;
        if (mapOptionNames.ContainsKey(strOptionNames[option]))
        {
            ShowOption(option);
        }
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
                //if (GUILayout.Button("Add Item"))
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

            /*
             나중에 지우세요
            if (addedObj.transform.childCount > 0)
            {
                GameObject child = addedObj.transform.GetChild(0).gameObject;
                //legOptions = child.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight();
            }
            */
            currentSkinnedMesh = GetSkinnedMesh();
            if (currentSkinnedMesh != null)
            {
                Debug.Log("Found");
            }
        }
    }
    private void OnChangeSlide(int index, float val)
    {
        val *= WEIGHT_SCALE;
        currentBlendOption[index] = val;
        if (currentSkinnedMesh != null)
        {
            if (currentSkinnedMesh.sharedMesh.blendShapeCount > index)
            {
                currentSkinnedMesh.SetBlendShapeWeight(index, val);
            }
        }
    }
    public void OnChangeSlide1(float val)
    {
        Debug.Log("slide 1 is: " + val);
        OnChangeSlide(0, val);
    }

    public void OnChangeSlide2(float val)
    {
        Debug.Log("slide 2 is: " + val);
        OnChangeSlide(1, val);
    }

    public void OnChangeSlide3(float val)
    {
        Debug.Log("slide 3 is: " + val);
        OnChangeSlide(2, val);
    }

    public void OnChangeSlide4(float val)
    {
        Debug.Log("slide 4 is: " + val);
        OnChangeSlide(3, val);
    }

    public void OnChangeSlide5(float val)
    {
        Debug.Log("slide 5 is: " + val);
        OnChangeSlide(4, val);
    }

    public void OnChangeSlide6(float val)
    {
        Debug.Log("slide 6 is: " + val);
        OnChangeSlide(5, val);
    }
}
