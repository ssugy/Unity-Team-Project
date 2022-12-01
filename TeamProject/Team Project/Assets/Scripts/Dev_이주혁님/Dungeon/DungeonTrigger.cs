using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrigger : MonoBehaviour
{
    public int num;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Evasion") || other.CompareTag("Dead") || other.CompareTag("Attacked"))
        {
            DungeonManager.instance.dungeonExplanation.text = DungeonManager.instance.explanationList[num];
            DungeonManager.instance.dungeonProgress.fillAmount += DungeonManager.instance.progressAmount;
            Destroy(gameObject);
        }
    }
}
