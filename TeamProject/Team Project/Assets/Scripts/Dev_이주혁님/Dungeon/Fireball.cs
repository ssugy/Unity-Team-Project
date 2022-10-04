using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 8f);
    }
    private void OnEnable()
    {
        Invoke("DestroySelf", 4f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Terrain")|| other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.IsAttacked(damage);
                Destroy(gameObject);
            }
        }
    }
}
