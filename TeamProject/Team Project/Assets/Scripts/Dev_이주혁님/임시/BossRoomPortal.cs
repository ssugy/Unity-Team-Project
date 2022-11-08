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
            // ĳ���� ��Ʈ�ѷ��� Ȱ��ȭ�Ǿ� ������ ��ġ �̵��� �Ұ�����.
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = destination.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
            
    }
}
