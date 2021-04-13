using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    EnergyBar blueTeamEnergyBar;
    EnergyBar redTeamEnergyBar;

    [SerializeField] int attackerCost = 2;
    [SerializeField] int defenderCost = 3;
    
    [SerializeField] GameObject soldierPrefab;


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
    }

    // Spawn soldier of a certain team of a certain role
    public void SpawnSoldier(SoldierTeam team, SoldierRole role)
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
                        }
                        break;

                    case SoldierRole.Defender:
                        if (blueTeamEnergyBar.energy >= defenderCost)
                        {
                            Debug.Log("Blue team defender spawned.");
                            blueTeamEnergyBar.SpendEnergy(defenderCost);
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
}
