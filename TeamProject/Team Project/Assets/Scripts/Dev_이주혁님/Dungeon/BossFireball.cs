using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : Fireball
{
    Vector3 targetVec;
    public GameObject Explosion;
    bool isCollision;
    SphereCollider col;
    private void Update()
    {
        targetVec = JY_CharacterListManager.s_instance.playerList[0].transform.position;
        if(!isCollision)
            transform.position = Vector3.MoveTowards(transform.position,targetVec,Time.deltaTime*10f);
    }
    private void OnEnable()
    {
        Invoke("DestroySelf", 4f);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Boss_FireBall);
        isCollision = false;
        col = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain") || other.CompareTag("Wall"))
        {
            isCollision = true;
            col.enabled = false;
            GameObject tmpExplosion = GameObject.Instantiate<GameObject>(Explosion, transform.position, Quaternion.identity);
            Invoke("DestroySelf", 1f);
        }
        else if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.IsAttacked(damage,null);
                Destroy(gameObject);
            }
        }
    }

}
