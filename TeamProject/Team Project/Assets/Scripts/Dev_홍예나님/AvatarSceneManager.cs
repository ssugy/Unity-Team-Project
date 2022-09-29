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

    // �� Item Group���� Option(n��)���� �����ϱ� ���� ������ �ʿ��մϴ�
    private List<float> currentBlendOption;
    private List<float> legOptions = new List<float>(); // Leg�� �ɼǵ�
    private List<float> hairOptions = new List<float>(); // hair�� �ɼǵ�
    private List<float> headOptions = new List<float>(); // head�� �ɼǵ�
    private List<float> torsoOptions = new List<float>(); // torso�� �ɼǵ�
    private SkinnedMeshRenderer currentSkinnedMesh = null;

    // Start is called before the first frame update
    void Start()
    {
        ShowCanvas();

        // 1. Item group(4��) ���� MapOptionNames Dictionary�� �ʱ�ȭ �մϴ�. 
        optionSubs = new List<int>();
        for(int i = 0; i < (int)MoreOptions.LAST; i++)
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
        //ù��° ��ư�� ���������� ������
        ShowOption(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //�ֿ�
            //ĳ���� ���� ��ư�� ���� ��� �κ������ �����͸� �ѱ�ϴ�.
            //�κ���� �����Ͽ� ĳ���͸���ŷ���� �������� ������ �κ������ �����͸� �ѱ�ϴ�.
            //��Ȳ�� ���� ���Ŀ� ���ӸŴ����� �����ϴ� �۾��� �ʿ��� �� �����ϴ�.
            if (JY_CharacterListManager.s_instance != null)
            {
                //�󽽷� ã�� �� �ۼ� �� break
                for (int i = 0; i < 4; i++)
                {
                    if (JY_CharacterListManager.s_instance.characterData.infoDataList[i].isNull == true)
                    {
                        if (CharacterNameInput!=null)
                        {
                            //�������ͽ� �ۼ�
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].name = CharacterNameInput.text;
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].isNull = false;
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].level = 1;
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].job = "����";
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].gender = (gender == 0 ? "M" : "F");
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].species = "�ΰ�";
                            //�𵨸� �ۼ�
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[0] = optionSubs[0];
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[1] = optionSubs[1];
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[2] = optionSubs[2];
                            JY_CharacterListManager.s_instance.characterData.infoDataList[i].characterAvatar[3] = optionSubs[3];
                            break;
                        }
                    }
                }
                //������ ���̺�
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
        // 1. strOptionNames ���� Item Group�� Name�� ����ִ�.
        //    �츮�� ���ϴ� Group�� �̸� �� option��ư�� ������ �̹� �˰��ִ�. (Hairs = 0, Heads, Torsos, Legs = 3)
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
                //if (GUILayout.Button("Add Item"))
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

            /*
             ���߿� ���켼��
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
