using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Player : MonoBehaviour
{    
    public int damage;
    public Collider target;
    public Transform player;

    private void Update()
    {
        if (target != null)
        {
            Vector3 tmp = target.transform.position - transform.position + new Vector3(0f, 1f, 0f);
            transform.Translate(tmp.normalized * Time.deltaTime * 12f, Space.World);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 12f);
        }
    }
    private void OnEnable()
    {
        Destroy(gameObject, 3f);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.IsAttacked(damage, Vector3.zero);
                enemy.target = player;
                Destroy(gameObject);
            }
        }
    }
}
