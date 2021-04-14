/*
 * While there is no need to write an AI to play the enemy part,
 * I decided to write a very simple one, consisting of spawning a soldier randomly at a certain interval.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    SoldierManager soldierManager;
    [SerializeField] GameManager gameManager;

    float spawnInterval = 10f;
    float timeToNextSpawn;

    // Start is called before the first frame update
    void Start()
    {
        soldierManager = GetComponent<SoldierManager>();
        timeToNextSpawn = Time.time + spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn a soldier at a random location every 10 seconds, if the energy is sufficient.
        if (IsInMatch())
        {
            if (Time.time >= timeToNextSpawn)
            {
                if (IsAttacker())
                {
                    soldierManager.SpawnSoldier(SoldierManager.SoldierTeam.Red, SoldierManager.SoldierRole.Attacker, RandomLocation());
                }
                else
                {
                    soldierManager.SpawnSoldier(SoldierManager.SoldierTeam.Red, SoldierManager.SoldierRole.Defender, RandomLocation());
                }
                timeToNextSpawn = Time.time + spawnInterval;
            }
        }
    }

    bool IsInMatch()
    {
        if (gameManager.GetGameState() == GameManager.GameState.BlueAttack || gameManager.GetGameState() == GameManager.GameState.RedAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsAttacker()
    {
        if (gameManager.GetGameState() == GameManager.GameState.BlueAttack)
        {
            return false;
        }
        else
        {
            if (gameManager.GetGameState() == GameManager.GameState.RedAttack)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 RandomLocation()
    {
        return new Vector3(Random.Range(10f, -10f), 0.75f, Random.Range(5f, 15f));
    }
}
