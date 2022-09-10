using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamRaycast : MonoBehaviour
{
    public Transform player;
    float distance;
    Renderer obstacle;
    Material obsMat;
    Color obsColor;

    private void Start()
    {
        obstacle = null;
        obsMat = null;        
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        Vector3 dir = Vector3.Normalize(player.position - transform.position);
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position,dir,out hitInfo, distance))
        {
            obstacle = hitInfo.collider.gameObject.GetComponent<Renderer>();
            if (obstacle != null)
            {
                obsMat = obstacle.material;                
                obsColor = obsMat.color;
                obsColor.a = 0.3f;
                obsMat.color = obsColor;
            }
        }
        else
        {
            if(obstacle != null)
            {
                obsColor.a = 1f;
                obsMat.color = obsColor;
            }
        }
    }
}
