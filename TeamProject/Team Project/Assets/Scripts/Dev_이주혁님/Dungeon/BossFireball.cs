using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : Fireball
{
    public Transform target;

    public GameObject impact;
    public GameObject explosion;
    Collider col;

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                target.position + new Vector3(0, 1f, 0), Time.deltaTime * 8f);
        }

            
    }
    private void OnEnable()
    {
        col = GetComponent<Collider>();
        Destroy(gameObject, 4f);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Boss_FireBall);                
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
            Instantiate(impact, transform.position, Quaternion.identity);
            
        }
        else if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.IsAttacked(damage, col);
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}
