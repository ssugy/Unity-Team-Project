using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_CharacterPlayData : MonoBehaviour
{

    public Text nameText;
    public Text levelText;
    public Text nameText_P;
    public Text levelText_P;
    public Image portraitImage;

    int index;
    // Start is called before the first frame update
    void Start()
    {
        if (JY_CharacterListManager.s_instance != null)
        {
            index = JY_CharacterListManager.s_instance.selectNum;
            LoadCharacterData();
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadCharacterData()
    {
        nameText.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[index].name;
        int level = JY_CharacterListManager.s_instance.jInfoData.infoDataList[index].level;
        levelText.text = "Lv."+level.ToString();
        nameText_P.text = nameText.text;
        levelText_P.text = "·¹º§:" + level.ToString();
        portraitImage.sprite = JY_CharacterListManager.s_instance.selectPortrait;
    }
}
