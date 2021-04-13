using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameManager game;

    // Start is called before the first frame update
    void Start()
    {
        game.OnMatchStart += HandleOnMatchStart;
    }
    
    void HandleOnMatchStart(object sender, System.EventArgs e)
    {
        StartCoroutine(DelayBallSpawning());
    }

    private void SpawnBall()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayBallSpawning() // Delay the spawning a bit, till the game state changes
    {
        yield return new WaitForSeconds(0.001f);
        SpawnBall();
    }
}
