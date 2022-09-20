using CartoonHeroes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static CartoonHeroes.SetCharacter;
using static GameManager;

public class AvatarSceneManager : MonoBehaviour
{
    private enum Steps{ SELECT_JOB, SELECT_BODY, SELECT_NAME, SAVE}
    private enum MoreOptions { FACE, HAIR, TORSO, LEGS, LAST}
    public enum Gender { MALE, FEMALE, NEUTRAL}
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
    private SetCharacter targetScript;
    private Dictionary<string, int> mapOptionNames;
    private string[] strOptionNames = {"Heads", "Hairs", "Torsos", "Legs" };

    // Start is called before the first frame update
    void Start()
    {
        ShowCanvas();

        optionSubs = new List<int>();
        for(int i = 0; i < (int)MoreOptions.LAST; i++)
        {
            optionSubs.Add(0);
        }
        mapOptionNames = new Dictionary<string, int>();

        gender = Gender.MALE;
        ShowCharacter();
        ShowOption();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMap()
    {
        mapOptionNames.Clear();
        int i = 0;
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
        if(currentStep < Steps.SAVE)
        {
            currentStep++;
            ShowCanvas();
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
    }

    public void OnClickMale()
    {
        gender = Gender.MALE;
        ShowCharacter();
    }

    private void ShowOption()
    {
        HideAllOption();

        options[currentOptionPanel].SetActive(true);

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
            currentOption = (MoreOptions)mapOptionNames[strOptionNames[option]];
            ShowOption();
        }
    }

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

    public void OnClickSubOption(int sub)
    {
        int i = (int)currentOption;
        DeleteSubOption();
        {
            optionSubs[i] = sub;
            GameObject addedObj = targetScript.AddItem(targetScript.itemGroups[i], optionSubs[i]);
        }
    }
}
