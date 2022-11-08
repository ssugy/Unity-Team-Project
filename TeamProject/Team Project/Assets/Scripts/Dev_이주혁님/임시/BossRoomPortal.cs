using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomPortal : MonoBehaviour
{
    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // 캐릭터 콘트롤러가 활성화되어 있으면 위치 이동이 불가능함.
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = destination.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
            
    }
}
