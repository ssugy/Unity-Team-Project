using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Drop : MonoBehaviour
{
    private Enemy enemy;
    public Door door;      // �ش� ���谡 �� ��.
    public GameObject message;
    bool keyDrop;
    
            

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        keyDrop = false;
    }

    public void PickUpKey()
    {
        message.SetActive(true);
        door.isLocked = false;
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Key);
    }
    private void Update()
    {
        if (enemy.curHealth <= 0 && !keyDrop) 
        {
            PickUpKey();
            keyDrop = true;
        }
    }
}
