using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    /*
     *  TODO:
     *  Show indicator:
     *      - Direction of soldiers
     *      - Highlight of attacker holding the ball
     *  
     *  Attacker and defender collision
     *  
     *  Behavior when inactivated
     *  
     *  Defender vision/aggro circle
     *  
     *  Animations:
     *      - Defender caught the attacker
     *      - On spawn behavior
     *      - When attacker is at opponent fence
     * 
     *  Others (Check within the functions
     */

    #region Parameters
    public enum Role
    {
        Attacker,
        Defender
    };

    bool isActivated = false;
    bool isHoldingBall = false;

    Role soldierRole;

    float normalSpeed;
    float carryingSpeed;
    float returnSpeed;

    readonly float SPAWNTIME = 0.5f;
    float reactivateTime;

    Vector3 originLocation;

    [SerializeField] GameObject opponentGate;

    #endregion

    #region Visuals

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        // (For use by defender) Set the origin location of the soldier
        originLocation = transform.position;

        // Set soldier parameters, depending on whether it is attacking or defending
        switch (soldierRole)
        {
            case Role.Attacker:
                normalSpeed = 1.5f;
                carryingSpeed = 0.75f;
                reactivateTime = 2.5f;
                break;

            case Role.Defender:
                normalSpeed = 1.0f;
                returnSpeed = 2f;
                reactivateTime = 4f;
                break;
        }

        StartCoroutine(Reactivation(SPAWNTIME));
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            // Attacker behavior
            if (soldierRole == Role.Attacker)
            {
                // Chase the ball if it is not being held
                // Move(normalSpeed, FindObjectOfType<Ball>().transform.position);

                // When holding the ball
                if (isHoldingBall)
                {
                    Move(carryingSpeed, opponentGate.transform.position);
                }

                // When there is no ball to chase or hold
                Move(normalSpeed);
            }
            else if (soldierRole == Role.Defender)
            {
                // On standby

                // Chase when the attacker with ball enters detection circle

            }
        }
    }

    #endregion

    #region Logic

    void SetSoldierRole(Role role)
    {
        soldierRole = role;
    }

    void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void Move(float speed, Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    IEnumerator Reactivation(float activationTime)
    {
        yield return new WaitForSeconds(activationTime);
        // Activate the soldier
        isActivated = true;
    }

    #endregion
}
