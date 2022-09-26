using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{

    public int hp;
    public int currentHp;
    public int atk;
    public int def;


    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;

    }

    public int Hit(int _playerAtk)
    {

        int playerAtk = _playerAtk;
        int dmg;
        if (def >= playerAtk)
            dmg = 1;
        else
            dmg = playerAtk - def;

        currentHp -= dmg;

        if(currentHp <= 0)
        {
            Destroy(this.gameObject);
        }

        return dmg;
    }
}
