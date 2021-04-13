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

    public MeshRenderer Renderer;
    Material material;

    Color originalColor;

    bool isActivated;

    SoldierManager soldierManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get the original color of the soldier for when it is reactivated
        material = Renderer.material;
        originalColor = material.color;

        // Set its status to inactivated
        isActivated = false;

        soldierManager = GameObject.Find("GameManager").GetComponent<SoldierManager>();
        soldierManager.HandleInactivation(SoldierManager.SoldierRole.Attacker, SoldierManager.ReactivationType.Spawning);
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

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {

        }
    }

    void DeactivateSoldier()
    {
        // Make it gray
        material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            return;
        }
        else
        {

        }
    }
}
