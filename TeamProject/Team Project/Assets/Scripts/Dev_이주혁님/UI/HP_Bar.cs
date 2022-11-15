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
    }
    public void Recognize(Enemy _enemy)
    {
        enemy = _enemy;
        hpBar.SetActive(true);
    }    
    
    IEnumerator Return()
    {
        yield return new WaitForSeconds(1f);
        Enemy_HP_UI.ReturnObject(this);
    }
}
