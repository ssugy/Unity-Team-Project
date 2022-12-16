using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroOptionPannel : MonoBehaviour
{
    public GameObject introOptionPannel;
    public void ShowIntroOptionPannel()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        introOptionPannel.SetActive(true);
    }

    public void CloseOptionPannel()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_02);
        introOptionPannel.SetActive(false);
    }

    // ���ʿ� �ִ� ��� ��ư ������, �����ʿ� �ִ� ���� â�� ����ȴ�.
    public GameObject[] optionDetailPannels;    // �������� ���г�
    public Image[] optionMenuPannelImgs;    // ������ �޴� �г��� img������Ʈ
    public void ShowDetailPannel(GameObject go)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_02);
        int num = int.Parse(go.name.Split('_')[0]);
        for (int i = 0; i < optionDetailPannels.Length; i++)
        {
            if (i == num)
            {
                optionDetailPannels[i].SetActive(true);
                optionMenuPannelImgs[i].color = Color.red;
            }
            else
            {
                optionDetailPannels[i].SetActive(false);
                optionMenuPannelImgs[i].color = Color.white;
            }
        }
    }

    // ����, �ѱ���, �Ϻ���, ���Ͼ�, ��������, �߱���(�������� ��Ʈ�� ������ ����)
    private enum Langs
    {
        US,
        KR,
        JP,
        DE,
        FR,
        CN
    }
    private Langs selectedLang;
    // ������ �� �����ϸ� �ش� ���� ��ȯ�Ǵ� �Լ�
    public Image[] languageMenuImgs;
    public void SelectLanguage(GameObject go)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_02);
        int num = int.Parse(go.name.Split('_')[0]);
        GameManager.s_instance.currentLanguage = num;   // ���� ���õ� �� �˷��ֱ�.
        GameManager.s_instance.LocalizeChanged();   // ��ȯ�� ���� �����ϱ�.

        selectedLang = (Langs)num;
        for (int i = 0; i < languageMenuImgs.Length; i++)
        {
            if (i == num)
            {
                languageMenuImgs[i].color = Color.red;
            }
            else
            {
                languageMenuImgs[i].color = Color.white;
            }
        }
    }

    public void OnOffFrameCheck()
    {
        GameObject pannelObj = GameManager.s_instance.gameObject.transform.GetChild(1).gameObject;
        pannelObj.SetActive(!pannelObj.activeSelf);
        pannelObj.transform.GetChild(0).GetComponent<FPSChecker>().StartFPSChecker();
    }
}
