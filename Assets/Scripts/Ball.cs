using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    /*
     *  TODO:
     *      - When first spawned
     *      - When passed to other attacker
     *      - When in possession of an attacker
     *      - When inside goal gate
     */

    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject blueField;
    [SerializeField] GameObject redField;

    public bool isHeld = false;

    private void Start()
    {
        // Check what state is the game manager in, then set the location accordingly
        switch (gameManager.currentGameState)
        {
            case GameManager.GameState.PreGame:
                break;

            case GameManager.GameState.BlueAttack:
                transform.position = new Vector3(Random.Range(-10f, 10f), 0.75f, Random.Range(-15f, -1f));
                break;

            case GameManager.GameState.RedAttack:
                transform.position = new Vector3(Random.Range(-10f, 10f), 0.75f, Random.Range(1f, 15f));
                break;

            case GameManager.GameState.PenaltyGame:
                break;

            case GameManager.GameState.GameEnd:
                break;

            default:
                break;
        }
    }
}
