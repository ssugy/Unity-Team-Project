using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamRaycast : MonoBehaviour
{
    public Transform player;
    RaycastHit[] hits;
    int layerMask;

    private void Start()
    {
        player = PlayerController.player;
        hits = new RaycastHit[0];
        layerMask = 1 << LayerMask.NameToLayer("Obstacle");
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position + new Vector3(0, 0.9f, 0)) ;
        Vector3 dir = Vector3.Normalize(player.position + new Vector3(0, 0.9f, 0) - transform.position);
        
        
        for (int i = 0; i < hits.Length; ++i)
        {
            RaycastHit hitInfo = hits[i];
            MeshRenderer obstacle = hitInfo.transform.GetComponent<MeshRenderer>();
            if (obstacle)
            {
                Color tmpColor = obstacle.material.color;
                tmpColor.a = 1f;
                obstacle.material.color = tmpColor;
            }
        }

        hits = Physics.RaycastAll(transform.position, dir, distance, layerMask);
        for (int i = 0; i < hits.Length; ++i)
        {
            RaycastHit hitInfo = hits[i];
            MeshRenderer obstacle = hitInfo.transform.GetComponent<MeshRenderer>();
            if (obstacle)
            {
                Color tmpColor = obstacle.material.color;                
                tmpColor.a = 0.3f;
                obstacle.material.color = tmpColor;
            }
        }        
    }
}
