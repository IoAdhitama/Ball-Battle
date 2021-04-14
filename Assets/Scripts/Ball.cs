using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameManager game;
    [SerializeField] float ballSpeed = 1.5f;

    bool isHeld; // Whether it is currently being held
    bool hasBeenPickedUp; // Has it ever been held during a match? For win condition purpose

    string attackerTag = "Attacker";

    GameObject passTarget;
    Vector3 passTargetLocation;
    const int ATTACKER_LAYER = 10;

    // Start is called before the first frame update
    void Start()
    {
        // Event subscriptions
        game.OnMatchStart += HandleOnMatchStart;
        game.OnMatchEnd += HandleOnMatchEnd;
        game.OnBallPickedUp += HandleOnBallPickedUp;
        game.OnBallDropped += HandleOnBallDropped;
    }

    private void HandleOnBallDropped(object sender, System.EventArgs e)
    {
        transform.parent = null; // Un-child the ball
        isHeld = false;
        PassBall();
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            gameObject.layer = 9;
            isHeld = false;
        }
        else gameObject.layer = 12;

        if (passTarget != null)
        {
            Debug.Log("Passing ball");
            transform.LookAt(passTargetLocation);
            transform.Translate(Vector3.forward * ballSpeed * Time.deltaTime);
        }
    }

    private void HandleOnBallPickedUp(object sender, System.EventArgs e)
    {
        passTarget = null; // Nullify pass target when it is held (As it has no need to pass the ball)
        // game.ballDropped = false;
        isHeld = true;
        hasBeenPickedUp = true;
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

    private void PassBall()
    {
        Debug.Log("Ball passed.");
        if (passTarget == null)
        {
            FindNearestTarget();
        }
    }

    private void FindNearestTarget()
    {
        List<GameObject> validTargets = new List<GameObject>();

        // Find the game objects by tag, then filter it by a specific layer
        GameObject[] targets = GameObject.FindGameObjectsWithTag(attackerTag);
        foreach (GameObject target in targets)
        {
            if (target.layer == ATTACKER_LAYER)
            {
                validTargets.Add(target);
            }
        }

        float distance = Mathf.Infinity;
        GameObject nearest = null;

        Debug.Log(validTargets.Count + " pass targets found.");

        if (validTargets.Count != 0)
        {
            foreach (GameObject target in validTargets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < distance)
                {
                    distance = distanceToTarget;
                    nearest = target;
                }
            }
            passTarget = nearest;
            passTargetLocation = nearest.transform.position;
        }
        else
        {
            if (hasBeenPickedUp == true)
            {
                game.allAttackerOut = true;
            }
                
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attacker")) // Make the ball the parent of the attacker that took the ball
        {
            Debug.Log("Ball collided with an attacker");
            transform.parent = other.transform;
        }

        if (other.CompareTag("Gate"))
        {
            game.ballInGoal = true;
        }
    }
}
