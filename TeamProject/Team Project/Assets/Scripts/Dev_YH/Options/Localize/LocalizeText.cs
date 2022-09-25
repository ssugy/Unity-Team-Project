using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * �ش� ��ũ��Ʈ�� ��ȯ�� �ʿ��� �ؽ�Ʈ�� ���̸�, Ű���� ������ �־� �Ѵ�.
 */
public class LocalizeText : MonoBehaviour
{
    public string keyText;

    private void Start()
    {
        LocalizeChange();
        GameManager.s_instance.LocalizeChanged += LocalizeChange;   // �׼ǿ� �߰�
    }

    private void OnDestroy()
    {
        GameManager.s_instance.LocalizeChanged -= LocalizeChange;   // �� ��ȯ ��, �̺�Ʈ ����Ѱ� ���ܽ�Ű��.
    }

    private void LocalizeChange()
    {
        if (GetComponent<Text>() != null)
        {
            GetComponent<Text>().text = ChangeText(keyText);
        }
    }

    /// <summary>
    /// ��ũ��Ʈ�� ���� �Էµ� Ű��, ����� �����͸� ���ؼ� �� �°� ��ȯ�� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="key">�ܺο��� �Էµ� Ű</param>
    /// <returns></returns>
    private string ChangeText(string key)
    {
        int keyIndex = GameManager.s_instance.languages[0].value.FindIndex(x => x.Equals(key));

        Debug.Log($"keyIndex : {keyIndex}");
        return GameManager.s_instance.languages[GameManager.s_instance.currentLanguage].value[keyIndex];
    }
}
