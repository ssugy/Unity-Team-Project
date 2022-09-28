using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] public PlayerStat playerStat;
    private void Start()
    {
        SetState();
    }
    public void InitializeStat()                  // Ω∫≈» √ ±‚»≠
    {
        playerStat.statPoint = (playerStat.level - 1) * 3;
        playerStat.InitialStat(playerStat.characterClass);
    }
    public void StatUp(Adjustable _stat)
    {        
        if (playerStat.statPoint > 0)
        {
            switch (_stat)
            {
                case Adjustable.health:
                    --playerStat.statPoint;
                    ++playerStat.health;
                    SetState();
                    break;
                case Adjustable.stamina:
                    --playerStat.statPoint;
                    ++playerStat.stamina;
                    SetState();
                    break;
                case Adjustable.strength:
                    --playerStat.statPoint;
                    ++playerStat.strength;
                    SetState();
                    break;
                case Adjustable.dexterity:
                    --playerStat.statPoint;
                    ++playerStat.dexterity;
                    SetState();
                    break;
            }                                  
        }              
    }       // Ω∫≈» ≈ı¿⁄
    public void SetState()
    {
        playerStat.HP = playerStat.health * 50 + playerStat.strength * 10;
        playerStat.SP = playerStat.stamina * 10 + playerStat.strength * 2;
        playerStat.criPro = (20f + Sigma(2f, 1.03f, playerStat.dexterity))/100f;
        playerStat.defMag = 1 - Mathf.Pow(1.02f, -playerStat.defPoint);
        playerStat.atkPoint = Mathf.CeilToInt(Sigma(10f, 1.02f, playerStat.strength) + Sigma(10, 1.1f, playerStat.dexterity));
    }
    public float Sigma(float a, float b, int c)
    {
        float tmp = 0;
        for (int i = 0; i <= c-1; i++)
        {
            tmp += a * Mathf.Pow(b, (float)-i);
            
        }        
        return tmp;
    }
    public int AttackDamage(float _atkMag, float _enemyDef)
    {        
        float _criDamage;
        if (Random.Range(0f, 1f) <= playerStat.criPro)
        {
            _criDamage = PlayerStat.criMag;
        }
        else
        {
            _criDamage = 1f;
        }
        int _damage = Mathf.CeilToInt(playerStat.atkPoint * _atkMag * _enemyDef * _criDamage * Random.Range(0.95f, 1.05f));
        return _damage;          
    }        
}
