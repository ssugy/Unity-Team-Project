using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCamera : MonoBehaviour
{
    private GameObject player;
    private Vector3 initPos;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        FindPlayer();
    }

    void FindPlayer()
    {
        player = JY_CharacterListManager.s_instance.playerList[0].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        if (player != null)
        {
            Vector3 rot = player.transform.rotation.eulerAngles;
            float y = - rot.y + 90f;
            Vector3 pos = player.transform.position + new Vector3(distance * Mathf.Cos(y * Mathf.Deg2Rad), initPos.y, distance * Mathf.Sin(y * Mathf.Deg2Rad));
            Vector3 rotCam = new Vector3(0, 270.0f - y, 0);
            transform.position = pos;
            transform.rotation = Quaternion.Euler(rotCam);
        }
    }
}
