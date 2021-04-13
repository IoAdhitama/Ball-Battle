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

    Vector3 originalPosition;
    Color originalColor;

    bool isActivated;
    bool isBallHeld;
    public bool isHoldingBall;
    public event EventHandler OnBallPickedUp;

    Vector3 gateDestination;
    Vector3 opponentFence;
    Transform opponentToChase;

    enum DeactivateReason
    {
        Spawn,
        Caught
    }

    public MeshRenderer Renderer;
    Material material;

    SoldierManager soldierManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get the original color of the soldier for when it is reactivated
        material = Renderer.material;

        // Get the original location of the soldier (for defenders)
        originalPosition = transform.position;

        soldierManager = GameObject.Find("GameManager").GetComponent<SoldierManager>();

        gameObject.tag = "Attacker";

        DeactivateSoldier(DeactivateReason.Spawn);
    }

    public void SetSoldierParameters(SoldierManager.SoldierTeam team, SoldierManager.SoldierRole role)
    {
        switch (team)
        {
            case SoldierManager.SoldierTeam.Blue:
                originalColor = new Vector4(0f, 1f, 1f, 1f);
                break;
            case SoldierManager.SoldierTeam.Red:
                originalColor = new Vector4(0.6415f, 0f, 0f, 1f);
                break;
            default:
                break;
        }

        switch (role)
        {
            case SoldierManager.SoldierRole.Attacker:
                gameObject.tag = "Attacker";
                break;
            case SoldierManager.SoldierRole.Defender:
                gameObject.tag = "Defender";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Find whether ball is being held or not

        if (!isActivated)
        {
            if (gameObject.CompareTag("Defender"))
            {
                // Return to its original location at a faster speed
                transform.LookAt(originalPosition);
                transform.Translate((originalPosition - transform.position).normalized * defenderReturnSpeed * Time.deltaTime);
            }
            else return;
        }
        else
        {
            if (gameObject.CompareTag("Attacker"))
            {
                if (!isBallHeld) // If ball is not being held by attacker
                {
                    // Chase the ball

                    // Send an event once ball is held
                }
                else
                {
                    if (isHoldingBall) // If the attacker is holding the ball
                    {
                        transform.LookAt(gateDestination);
                        transform.Translate((gateDestination - transform.position).normalized * carryingSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                if (gameObject.CompareTag("Defender"))
                {
                    if (opponentToChase != null)
                    {
                        transform.LookAt(opponentToChase);
                        transform.Translate((opponentToChase.position - transform.position).normalized * defenderNormalSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        opponentToChase = target.transform;
    }

    void DeactivateSoldier(DeactivateReason reason)
    {
        SoldierManager.SoldierRole soldierRole = gameObject.CompareTag("Attacker") ? SoldierManager.SoldierRole.Attacker : SoldierManager.SoldierRole.Defender;

        // Make it gray
        material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        isActivated = false;

        // Set off the reactivation sequence based on cause of deactivation
        switch (reason)
        {
            case DeactivateReason.Spawn:
                soldierManager.HandleInactivation(soldierRole, SoldierManager.ReactivationType.Spawning);
                break;

            case DeactivateReason.Caught:
                soldierManager.HandleInactivation(soldierRole, SoldierManager.ReactivationType.Reactivating);
                break;

            default:
                break;
        }
        soldierManager.OnReactivation += SoldierManager_OnReactivation;
    }

    private void SoldierManager_OnReactivation(object sender, System.EventArgs e)
    {
        // Set to activated
        isActivated = true;

        // Return the color to its original color
        material.color = originalColor;

        soldierManager.OnReactivation -= SoldierManager_OnReactivation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHoldingBall && other.CompareTag("Defender")) // Attacker holding a ball is caught by defender
        {
            // Event to pass the ball to another attacker

            DeactivateSoldier(DeactivateReason.Caught);
        }

        if (gameObject.CompareTag("Attacker") && other.CompareTag("Fence")) // Attacker reaches the fence
        {
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Defender") && other.gameObject.GetComponent<Soldier>().isHoldingBall) // Defender catches an attacker holding a ball
        {
            DeactivateSoldier(DeactivateReason.Caught);
        }
    }
}
