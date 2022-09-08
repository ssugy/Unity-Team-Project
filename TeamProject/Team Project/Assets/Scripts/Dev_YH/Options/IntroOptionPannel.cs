using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroOptionPannel : MonoBehaviour
{
    public GameObject introOptionPannel;
    public void ShowIntroOptionPannel()
    {
        introOptionPannel.SetActive(true);
    }

    public void CloseOptionPannel()
    {
        introOptionPannel.SetActive(false);
    }

    // 왼쪽에 있는 목록 버튼 누르면, 오른쪽에 있는 세부 창이 변경된다.
    public GameObject[] optionDetailPannels;    // 오른쪽의 상세패널
    public Image[] optionMenuPannelImgs;    // 왼쪽의 메뉴 패널의 img컴포넌트
    public void ShowDetailPannel(GameObject go)
    {
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

    private enum Langs
    {
        KR,
        JP,
        US,
        DE,
        FR,
        CN
    }
    private Langs selectedLang;
    // 각각의 언어를 선택하면 해당 언어로 변환되는 함수
    public Image[] languageMenuImgs;
    public void SelectLanguage(GameObject go)
    {
        int num = int.Parse(go.name.Split('_')[0]);
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
}
