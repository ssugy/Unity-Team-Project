using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class AvatarSceneManager : MonoBehaviour
{
    private enum Steps{ SELECT_JOB, SELECT_BODY, SELECT_NAME, SAVE}
    private Steps currentStep = Steps.SELECT_JOB;

    public GameObject popup;
    public List<GameObject> canvases;
    // Start is called before the first frame update
    void Start()
    {
        ShowCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
