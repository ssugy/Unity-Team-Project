using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Key : MonoBehaviour
{
    public Door door;      // 해당 열쇠가 열 문.    

    private void OnTriggerEnter(Collider other)
    {
        // 내가 아닌 다른 플레이어가 열쇠를 획득해도 작동함.
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
