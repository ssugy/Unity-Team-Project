using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_Exp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        JY_CharacterListManager.s_instance.playerList[0].questExp(1500);
        Destroy(gameObject);
    }
}
