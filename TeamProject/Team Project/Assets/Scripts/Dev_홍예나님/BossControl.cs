using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    public GameObject boss;
    public float targetRadius;
    private Animator anim;
    public GameObject player;
    float speed = 1.0f;
    bool isPlayerNear = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = boss.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear)
        {
            LookPlayer();
        }
        RaycastHit[] rayHits =
        Physics.SphereCastAll(transform.position,
                              targetRadius,
                              transform.forward,
                              0f,
                              LayerMask.GetMask("Player"));
        if (rayHits.Length > 0)
        {
            //Transform target = rayHits[0].transform;
            if(!isPlayerNear)
            {
                isPlayerNear = true;
                BossManager.GetInstance().SetNear(true);
            }
        }
        else
        {
            if(isPlayerNear)
            {
                isPlayerNear = false;
                BossManager.GetInstance().SetNear(false);
            }
        }
    }

    public void OnClickAnim(string action)
    {
        anim.Play(action);
    }

    public bool GetAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void LookPlayer()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = 0f;
        transform.rotation = Quaternion.Euler(rot);
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = true;
            BossManager.GetInstance().SetNear(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            BossManager.GetInstance().SetNear(false);
        }
    }
    */

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }
}
