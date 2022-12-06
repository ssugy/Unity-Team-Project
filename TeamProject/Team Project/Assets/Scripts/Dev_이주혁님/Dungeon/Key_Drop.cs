using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Drop : MonoBehaviour
{
    private Enemy enemy;
    public Door door;      // 해당 열쇠가 열 문.                       

    private void Start()
    {
        enemy = GetComponent<Enemy>();        
    }

    public void PickUpKey()
    {
        BattleUI.instance.getKey.gameObject.SetActive(true);
        door.isLocked = false;
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Key);
    }
    private void Update()
    {
        // 몬스터가 사망하면 실행됨.
        if (enemy.CurHealth <= 0) 
        {
            PickUpKey();
            Destroy(this);
        }
    }
}
