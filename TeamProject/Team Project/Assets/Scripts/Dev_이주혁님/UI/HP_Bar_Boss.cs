using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar_Boss : MonoBehaviour
{        
    public Image hp;
    public Enemy enemy;

    private void Awake()
    {
        enemy = null;
    }

    private void Update()
    {
        if (enemy != null)
        {                        
            hp.fillAmount = (float)enemy.curHealth / enemy.maxHealth;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    public void Recognize(Enemy _enemy)
    {
        enemy = _enemy;
        gameObject.SetActive(true);
    }
}
