using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    public Vector3 offset;
    public GameObject hpBar;
    public Image hp;
    public Enemy enemy;
    // 예나
    public BossControl boss;

    private void OnEnable()
    {
        StartCoroutine(Return());
    }
    private void Update()
    {
        if (enemy != null)
        {            
            Vector2 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position +offset);
            transform.position = screenPos;            
            hp.fillAmount = (float)enemy.curHealth / enemy.maxHealth;                        
        }       
        // 예나
        else if (boss != null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(boss.transform.position + offset);
            transform.position = screenPos;
            hp.fillAmount = (float)BossManager.instance.curHealth / BossManager.instance.maxHealth;
        }
    }
    public void Recognize(Enemy _enemy)
    {
        enemy = _enemy;
        hpBar.SetActive(true);
    }
    // 예나
    public void RecognizeBoss(BossControl _boss)
    {
        boss = _boss;
        hpBar.SetActive(true);
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(1f);
        Enemy_HP_UI.ReturnObject(this);
    }
}
