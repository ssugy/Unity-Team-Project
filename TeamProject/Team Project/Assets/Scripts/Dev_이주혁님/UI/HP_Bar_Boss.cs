using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar_Boss : MonoBehaviour
{        
    public Image hp;    
    public Text nameText;
    private Enemy enemy;

    private void Awake()
    {
        enemy = null;
    }

    private void Update()
    {
        if (enemy != null)
        {                        
            hp.fillAmount = (float)enemy.CurHealth / enemy.maxHealth;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    public void Recognize(Enemy _enemy)
    {
        this.gameObject.SetActive(true);
        enemy = _enemy;
        nameText.text = enemy.gameObject.name;        
    }
}
