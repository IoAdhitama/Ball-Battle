using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("(Re)Spawning attributes")]
    [SerializeField] float spawnTime = 0.5f;
    [SerializeField] float attackerReactivateTime = 2.5f;
    [SerializeField] float defenderReactivateTime = 4f;

    [Header("Move speed(s)")]
    [SerializeField] float attackerNormalSpeed = 1.5f;
    [SerializeField] float defenderNormalSpeed = 1.0f;
    [SerializeField] float carryingSpeed = 0.75f;
    [SerializeField] float defenderReturnSpeed = 2.0f;

    [SerializeField] GameObject aggroCircle;
    [SerializeField] GameObject directionIndicator;
    [SerializeField] GameObject ballHoldHighlight;

    const int BALLHOLDER_LAYER = 9;
    const int ATTACKER_LAYER = 10;
    const int DEFENDER_LAYER = 11;
    const int DEACTIVATED_LAYER = 13;

    Vector3 originalPosition;
    Color originalColor;

    bool isActivated;
    public bool isHoldingBall;

    Vector3 gateDestination;
    Transform opponentToChase;

    enum DeactivateReason
    {
        Spawn,
        Caught
    }

    public enum ReactivationType
    {
        Spawning,
        Reactivating
    }

    public enum SoldierRole
    {
        Attacker,
        Defender
    }

    public MeshRenderer Renderer;
    Material material;

    SoldierManager soldierManager;
    GameManager game;

    // Start is called before the first frame update
    void Start()
    {
        // Get the original color of the soldier for when it is reactivated
        material = Renderer.material;

        // Get the original location of the soldier (for defenders)
        originalPosition = transform.position;

        soldierManager = GameObject.Find("GameManager").GetComponent<SoldierManager>();
        game = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Subscribe to events
        game.OnBallPickedUp += Game_OnBallPickedUp;
        game.OnBallDropped += Game_OnBallDropped;
        game.OnMatchEnd += Game_OnMatchEnd;

        // Soldier is inactivated for a certain time after being spawned
        DeactivateSoldier(DeactivateReason.Spawn);
    }

    private void Game_OnBallDropped(object sender, EventArgs e)
    {
        soldierManager.isBallHeld = false;
    }

    private void Game_OnBallPickedUp(object sender, EventArgs e)
    {
        soldierManager.isBallHeld = true;
    }

    public void SetSoldierParameters(SoldierManager.SoldierTeam team, SoldierManager.SoldierRole role)
    {
        switch (team)
        {
            case SoldierManager.SoldierTeam.Blue:
                originalColor = new Vector4(0f, 1f, 1f, 1f);
                gateDestination = GameObject.Find("RedGate").GetComponent<Transform>().position;
                break;

            case SoldierManager.SoldierTeam.Red:
                originalColor = new Vector4(0.6415f, 0f, 0f, 1f);
                gateDestination = GameObject.Find("BlueGate").GetComponent<Transform>().position;
                break;

            default:
                break;
        }

        switch (role)
        {
            case SoldierManager.SoldierRole.Attacker:
                gameObject.tag = "Attacker";

                // Set the layer of the child objects (for collision detection)
                SetSoldierLayer(ATTACKER_LAYER);
                aggroCircle.SetActive(false);
                break;

            case SoldierManager.SoldierRole.Defender:
                gameObject.tag = "Defender";

                SetSoldierLayer(DEFENDER_LAYER);

                break;

            default:
                break;
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Disable the direction indicator by default
        directionIndicator.SetActive(false);

        // Find whether ball is being held or not, as well as its location
        Vector3 ballLocation = GetBallLocation();

        if (!isActivated)
        {
            if (gameObject.CompareTag("Defender"))
            {
                // Return to its original location at a faster speed
                Move(defenderReturnSpeed, originalPosition);
            }
            else return;
        }
        else
        {
            if (gameObject.CompareTag("Attacker"))
            {
                if (!soldierManager.isBallHeld) // If ball is not being held by attacker
                {
                    // Chase the ball
                    Move(attackerNormalSpeed, ballLocation);
                }
                else
                {
                    if (isHoldingBall) // If the attacker is holding the ball
                    {
                        Move(carryingSpeed, gateDestination);
                    }
                    else
                    {
                        Move(attackerNormalSpeed);
                    }
                }
            }
            else
            {
                if (gameObject.CompareTag("Defender"))
                {
                    // On standby, waiting for target
                    if (opponentToChase != null)
                    {
                        aggroCircle.SetActive(false);
                        Move(defenderNormalSpeed, opponentToChase.position);
                    }
                }
            }
        }
    }

    void Move(float speed) // Go straight forward
    {
        directionIndicator.SetActive(true); // Activate indicator when moving

        if (originalColor == new Color(0f, 1f, 1f, 1f)) // Soldier is colored blue
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        else // We can assume that it will be colored red, since soldier will not move while it's gray/inactivated anyway
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }

    void Move(float speed, Vector3 destination) // Move to a certain location
    {
        directionIndicator.SetActive(true); // Activate indicator when moving

        transform.LookAt(destination);
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Constrain the rotation of the object

    }

    private void SetSoldierLayer(int layer)
    {
        gameObject.layer = layer;
        transform.GetChild(0).gameObject.layer = layer;
    }

    Vector3 GetBallLocation()
    {
        return GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

    public void SetTarget(GameObject target)
    {
        opponentToChase = target.transform;
    }

    void DeactivateSoldier(DeactivateReason reason)
    {
        SoldierRole soldierRole = gameObject.CompareTag("Attacker") ? SoldierRole.Attacker : SoldierRole.Defender;

        // Make it gray
        material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        isActivated = false;

        // Set layer to deactivated so it can let every soldier pass through
        SetSoldierLayer(DEACTIVATED_LAYER);

        // Turn off indicators if any are active
        ballHoldHighlight.SetActive(false);

        // If the one deactivated is the attacker holding the ball, set event to pass the ball
        if (isHoldingBall)
        {
            game.ballDropped = true;
        }
        isHoldingBall = false;


        // Set off the reactivation sequence based on cause of deactivation
        switch (reason)
        {
            case DeactivateReason.Spawn:
                HandleInactivation(soldierRole, ReactivationType.Spawning);
                break;

            case DeactivateReason.Caught:
                HandleInactivation(soldierRole, ReactivationType.Reactivating);
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

    private void ReactivateSoldier()
    {
        // Set to activated
        isActivated = true;

        // Return the color to its original color
        material.color = originalColor;

        // Return its layer to the original one
        if (gameObject.CompareTag("Attacker"))
        {
            SetSoldierLayer(ATTACKER_LAYER);
        }
        else
        {
            SetSoldierLayer(DEFENDER_LAYER);
        }
    }

    IEnumerator ReactivateCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        ReactivateSoldier();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            game.ballIsPickedUp = true;
            isHoldingBall = true;
            SetSoldierLayer(BALLHOLDER_LAYER);
    

            ballHoldHighlight.SetActive(true);
        }

        if (isHoldingBall && other.CompareTag("Defender")) // Attacker holding a ball is caught by defender
        {
            DeactivateSoldier(DeactivateReason.Caught);
        }

        if (gameObject.CompareTag("Attacker") && other.CompareTag("Fence")) // Attacker reaches the fence
        {
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Defender") && other.gameObject.GetComponentInParent<Soldier>().isHoldingBall) // Defender catches an attacker holding a ball
        {
            DeactivateSoldier(DeactivateReason.Caught);

            // Only this trigger, where the defender caught the attacker is fired, so we deactivate the other soldier as well.
            other.gameObject.GetComponentInParent<Soldier>().DeactivateSoldier(DeactivateReason.Caught);
        }
    }

    private void Game_OnMatchEnd(object sender, EventArgs e)
    {
        game.OnBallPickedUp -= Game_OnBallPickedUp;
        game.OnMatchEnd -= Game_OnMatchEnd;
        Destroy(gameObject);
    }
}
