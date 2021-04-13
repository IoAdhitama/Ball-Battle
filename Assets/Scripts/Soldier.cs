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
    bool isHoldingBall;

    enum SoldierRole
    {
        Attacker,
        Defender
    }
    SoldierManager.SoldierRole soldierRole;

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

        soldierRole = role;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            
        }
    }

    void DeactivateSoldier(DeactivateReason reason)
    {
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
        if (!isActivated)
        {
            return;
        }
        else
        {
            // Collides with a ball

            // Collides with other team/role
            if (other.GetComponent<Soldier>().soldierRole != this.soldierRole)
            {
                DeactivateSoldier(DeactivateReason.Caught);
            }
        }
    }
}
