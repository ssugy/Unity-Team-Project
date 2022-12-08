using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Key : MonoBehaviour
{
    public Door door;      // �ش� ���谡 �� ��.    

    private void OnTriggerEnter(Collider other)
    {
        // ���� �ƴ� �ٸ� �÷��̾ ���踦 ȹ���ص� �۵���.
        if (other.CompareTag("Player"))
        {
            PickUpKey();
        }        
    }
    public void PickUpKey()
    {
        BattleUI.instance.getKey.gameObject.SetActive(true);
        door.isLocked = false;
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Key);
        Destroy(gameObject);

        if (SceneManager.GetActiveScene().name == "06. Dungeon_Fire")
        {
            DungeonManager.instance.DungeonProgress(1);
            DungeonManager.instance.SetDungeonGuide(1);
        }
    }
}
