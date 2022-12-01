using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NameCheck : MonoBehaviour
{
    const int MAX_NAMELEGTH = 10;
    InputField field;
    Text text;
    private string oldValue = "";
    
    void Start()
    {
        field = GetComponent<InputField>();
        text = field.textComponent;
    }

    public void OnValueChanged(string str)
    {
        if (text != null)
        {
            //string str = text.text;
            if (str.Length > MAX_NAMELEGTH || HasSpecialChars(str))
            {
                field.text = oldValue;
                //Debug.Log(str);
            }
            else
            {
                oldValue = str;
            }
        }
    }

    private bool HasSpecialChars(string yourString)
    {
        // ���ڿ��� ���ڳ� ���ڰ� �ƴ� Ư�����ڰ� ������ true�� ��ȯ.
        return yourString.Any(ch => !Char.IsLetterOrDigit(ch));
    }

}
