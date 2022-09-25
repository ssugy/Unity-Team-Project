using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 해당 스크립트는 변환이 필요한 텍스트에 붙이며, 키값을 지정해 둬야 한다.
 */
public class LocalizeText : MonoBehaviour
{
    public string keyText;

    private void Start()
    {
        LocalizeChange();
        GameManager.s_instance.LocalizeChanged += LocalizeChange;   // 액션에 추가
    }

    private void OnDestroy()
    {
        GameManager.s_instance.LocalizeChanged -= LocalizeChange;   // 씬 전환 시, 이벤트 등록한것 제외시키기.
    }

    private void LocalizeChange()
    {
        if (GetComponent<Text>() != null)
        {
            GetComponent<Text>().text = ChangeText(keyText);
        }
    }

    /// <summary>
    /// 스크립트에 직접 입력된 키와, 저장된 데이터를 비교해서 언어에 맞게 변환된 문장을 반환하는 함수
    /// </summary>
    /// <param name="key">외부에서 입력된 키</param>
    /// <returns></returns>
    private string ChangeText(string key)
    {
        int keyIndex = GameManager.s_instance.languages[0].value.FindIndex(x => x.Equals(key));

        Debug.Log($"keyIndex : {keyIndex}");
        return GameManager.s_instance.languages[GameManager.s_instance.currentLanguage].value[keyIndex];
    }
}
