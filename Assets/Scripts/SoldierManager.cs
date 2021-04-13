using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    EnergyBar blueTeamEnergyBar;
    EnergyBar redTeamEnergyBar;

    [SerializeField] GameObject soldierPrefab;

    [Header("Soldier Stats")]
    [SerializeField] int attackerCost = 2;
    [SerializeField] int defenderCost = 3;

    [Header("(Re)Spawning attributes")]
    [SerializeField] float spawnTime = 0.5f;
    [SerializeField] float attackerReactivateTime = 2.5f;
    [SerializeField] float defenderReactivateTime = 4f;

    [Header("Move speed(s)")]
    [SerializeField] float attackerNormalSpeed = 1.5f;
    [SerializeField] float defenderNormalSpeed = 1.0f;
    [SerializeField] float carryingSpeed = 0.75f;
    [SerializeField] float defenderReturnSpeed = 2.0f;

    public event EventHandler OnReactivation;


    public enum SoldierRole
    {
        Attacker,
        Defender
    }

    public enum SoldierTeam
    {
        Blue,
        Red
    }

    public enum ReactivationType
    {
        Spawning,
        Reactivating
    }

    private void Awake()
    {
        blueTeamEnergyBar = GameObject.Find("PlayerEnergyBar").GetComponent<EnergyBar>();
        redTeamEnergyBar = GameObject.Find("EnemyEnergyBar").GetComponent<EnergyBar>();
    }

    // Spawn soldier of a certain team of a certain role
    public void SpawnSoldier(SoldierTeam team, SoldierRole role, Vector3 position)
    {
        switch (team)
        {
            case SoldierTeam.Blue:
                switch (role)
                {
                    case SoldierRole.Attacker:
                        if (blueTeamEnergyBar.energy >= attackerCost)
                        {
                            Debug.Log("Blue team attacker spawned.");
                            blueTeamEnergyBar.SpendEnergy(attackerCost);

                            GameObject soldier = Instantiate(soldierPrefab, position, Quaternion.identity);
                            soldier.GetComponent<Soldier>().SetSoldierParameters(team, role);
                        }
                        break;

                    case SoldierRole.Defender:
                        if (blueTeamEnergyBar.energy >= defenderCost)
                        {
                            Debug.Log("Blue team defender spawned.");
                            blueTeamEnergyBar.SpendEnergy(defenderCost);

                            GameObject soldier = Instantiate(soldierPrefab, position, Quaternion.identity);
                            soldier.GetComponent<Soldier>().SetSoldierParameters(team, role);
                        }
                        break;

                    default:
                        break;
                }
                break;

            case SoldierTeam.Red:
                switch (role)
                {
                    case SoldierRole.Attacker:
                        if (redTeamEnergyBar.energy >= attackerCost)
                        {
                            Debug.Log("Red team attacker spawned.");
                            redTeamEnergyBar.SpendEnergy(attackerCost);
                        }
                        break;

                    case SoldierRole.Defender:
                        if (blueTeamEnergyBar.energy >= defenderCost)
                        {
                            Debug.Log("Red team defender spawned.");
                            blueTeamEnergyBar.SpendEnergy(defenderCost);
                        }
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }

    public void HandleInactivation(SoldierRole role, ReactivationType type)
    {
        // Reactivation sequence for the soldier
        switch (type)
        {
            case ReactivationType.Spawning:
                StartCoroutine(ReactivateCountdown(spawnTime));
                break;

            case ReactivationType.Reactivating:
                switch (role)
                {
                    case SoldierRole.Attacker:
                        StartCoroutine(ReactivateCountdown(attackerReactivateTime));
                        break;

                    case SoldierRole.Defender:
                        StartCoroutine(ReactivateCountdown(defenderReactivateTime));
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator ReactivateCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        ReactivateSoldier();
    }

    void ReactivateSoldier()
    {
        // Reactivate the soldier
        OnReactivation?.Invoke(this, EventArgs.Empty);
    }
}
