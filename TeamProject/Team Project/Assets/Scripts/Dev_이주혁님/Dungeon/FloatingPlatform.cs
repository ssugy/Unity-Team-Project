using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveSpeed;
    private CharacterController player;    
    
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.localPosition, endPos) < 0.1f)
        {
            Vector3 tmp = startPos;
            startPos = endPos;
            endPos = tmp;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Evasion") || other.CompareTag("Dead") || other.CompareTag("Attacked")) 
        {
            player = other.GetComponent<CharacterController>();
            Player.instance.isGround = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            player.Move((endPos - transform.localPosition).normalized * Time.deltaTime * moveSpeed);
            Player.instance.isGround = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        player = null;
    }
}
