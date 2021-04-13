﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask layerMask;

    SoldierManager soldierManager;
    [SerializeField] GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        soldierManager = GetComponent<SoldierManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input, check whether it is a valid location for spawning soldiers
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Check the state of the game, then react accordingly
            if (IsInMatch())
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
                {
                    if (IsAttacker())
                    {
                        soldierManager.SpawnSoldier(SoldierManager.SoldierTeam.Blue, SoldierManager.SoldierRole.Attacker);
                    }
                    else
                    {
                        soldierManager.SpawnSoldier(SoldierManager.SoldierTeam.Blue, SoldierManager.SoldierRole.Defender);
                    }
                }
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
            return true;
        }
        else
        { return false; }
    }
}
