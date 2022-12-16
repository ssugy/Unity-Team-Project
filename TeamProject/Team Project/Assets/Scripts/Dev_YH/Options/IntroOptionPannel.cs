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

    // 왼쪽에 있는 목록 버튼 누르면, 오른쪽에 있는 세부 창이 변경된다.
    public GameObject[] optionDetailPannels;    // 오른쪽의 상세패널
    public Image[] optionMenuPannelImgs;    // 왼쪽의 메뉴 패널의 img컴포넌트
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

    // 영어, 한국어, 일본어, 독일어, 프랑스어, 중국어(스프레드 시트와 동일한 순서)
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
    // 각각의 언어를 선택하면 해당 언어로 변환되는 함수
    public Image[] languageMenuImgs;
    public void SelectLanguage(GameObject go)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_02);
        int num = int.Parse(go.name.Split('_')[0]);
        GameManager.s_instance.currentLanguage = num;   // 현재 선택된 언어를 알려주기.
        GameManager.s_instance.LocalizeChanged();   // 변환된 언어로 변경하기.

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
