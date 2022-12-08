using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_Guide : MonoBehaviour
{
    public Transform target_1;
    Player player;
    int state;
    // Start is called before the first frame update
    void Start()
    {
        player = JY_CharacterListManager.s_instance.playerList[0];
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //calcVec(target_1);
    }
    void calcVec(Transform target)
    {
        Vector3 dir = (target.position-player.transform.position).normalized;
        dir.y = 0f;
        Vector3 tmp = dir + player.transform.position;
        tmp.y += 0.5f;
        transform.position = tmp;
        transform.LookAt(target);
    }
}
