using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveSpeed;    

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
        if (other.CompareTag("Player") || other.CompareTag("Evasion") 
            || other.CompareTag("Dead") || other.CompareTag("Attacked"))         
            other.GetComponent<Player>().isGround = true;
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Evasion") 
            || other.CompareTag("Dead") || other.CompareTag("Attacked"))
        {
            other.GetComponent<Player>().isGround = true;
            other.GetComponent<CharacterController>().
                Move((endPos - transform.localPosition).normalized * Time.deltaTime * moveSpeed);
        }                           
    }    
}
