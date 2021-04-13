﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameManager game;

    bool isHeld; // Whether it is currently being held
    bool hasBeenPickedUp; // Has it ever been held during a match? For win condition purpose

    // Start is called before the first frame update
    void Start()
    {
        game.OnMatchStart += HandleOnMatchStart;
        game.OnMatchEnd += HandleOnMatchEnd;


    }
    
    void HandleOnMatchStart(object sender, System.EventArgs e)
    {
        StartCoroutine(DelayBallSpawning());
    }

    void HandleOnMatchEnd(object sender, System.EventArgs e)
    {
        ResetBall();
    }

    private void SpawnBall() // Generate (teleport) the ball to a random location on the attacker's field
    {
        switch (game.GetGameState())
        {
            case GameManager.GameState.BlueAttack:
                transform.position = new Vector3(Random.Range(10f, -10f), 0.75f, Random.Range(-15f, -5f));
                break;

            case GameManager.GameState.RedAttack:
                transform.position = new Vector3(Random.Range(10f, -10f), 0.75f, Random.Range(5f, 15f));
                break;

            case GameManager.GameState.PenaltyGame:
                break;

            default:
                break;
        }
    }

    private void ResetBall() // Reset the ball's conditions and location
    {
        transform.position = Vector3.zero;
        isHeld = false;
        hasBeenPickedUp = false;
    }

    IEnumerator DelayBallSpawning() // Delay the spawning a bit, till the game state changes
    {
        yield return new WaitForSeconds(0.001f);
        SpawnBall();
    }
}
