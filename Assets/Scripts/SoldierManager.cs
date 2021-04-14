using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    EnergyBar blueTeamEnergyBar;
    EnergyBar redTeamEnergyBar;

    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject enemySoldierPrefab;

    [Header("Soldier Stats")]
    [SerializeField] int attackerCost = 2;
    [SerializeField] int defenderCost = 3;

    public bool isBallHeld;

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

    private void Awake()
    {
        blueTeamEnergyBar = GameObject.Find("PlayerEnergyBar").GetComponent<EnergyBar>();
        redTeamEnergyBar = GameObject.Find("EnemyEnergyBar").GetComponent<EnergyBar>();

        GameManager game = GetComponent<GameManager>();
        game.OnBallDropped += Game_OnBallDropped;
        game.OnMatchEnd += Game_OnMatchEnd;
    }

    private void Game_OnMatchEnd(object sender, EventArgs e)
    {
        isBallHeld = false;
    }

    private void Game_OnBallDropped(object sender, EventArgs e)
    {
        isBallHeld = false;
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
                            blueTeamEnergyBar.SpendEnergy(attackerCost);

                            GameObject soldier = Instantiate(soldierPrefab, position, Quaternion.identity);
                            soldier.GetComponent<Soldier>().SetSoldierParameters(team, role);
                        }
                        break;

                    case SoldierRole.Defender:
                        if (blueTeamEnergyBar.energy >= defenderCost)
                        {
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
                            redTeamEnergyBar.SpendEnergy(attackerCost);

                            GameObject soldier = Instantiate(enemySoldierPrefab, position, Quaternion.identity);
                            soldier.GetComponent<Soldier>().SetSoldierParameters(team, role);
                        }
                        break;

                    case SoldierRole.Defender:
                        if (redTeamEnergyBar.energy >= defenderCost)
                        {
                            redTeamEnergyBar.SpendEnergy(defenderCost);

                            GameObject soldier = Instantiate(enemySoldierPrefab, position, Quaternion.identity);
                            soldier.GetComponent<Soldier>().SetSoldierParameters(team, role);
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
}
