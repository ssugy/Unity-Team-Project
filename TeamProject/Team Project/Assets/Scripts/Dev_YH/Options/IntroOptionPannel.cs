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

    // ���ʿ� �ִ� ��� ��ư ������, �����ʿ� �ִ� ���� â�� ����ȴ�.
    public GameObject[] optionDetailPannels;    // �������� ���г�
    public Image[] optionMenuPannelImgs;    // ������ �޴� �г��� img������Ʈ
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
    // ������ �� �����ϸ� �ش� ���� ��ȯ�Ǵ� �Լ�
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
