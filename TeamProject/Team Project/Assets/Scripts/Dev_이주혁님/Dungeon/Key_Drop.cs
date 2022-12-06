using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Drop : MonoBehaviour
{
    private Enemy enemy;
    public Door door;      // �ش� ���谡 �� ��.                       

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
        // ���Ͱ� ����ϸ� �����.
        if (enemy.CurHealth <= 0) 
        {
            PickUpKey();
            Destroy(this);
        }
    }
}
