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

    bool isactivated = false;

    Role role;

    float normalSpeed;
    float carryingSpeed;
    float returnSpeed;

    readonly float SPAWNTIME = 0.5f;
    float reactivateTime;

    Vector3 originLocation;

    #endregion

    #region Functions

    // Start is called before the first frame update
    void Start()
    {
        // (For use by defender) Set the origin location of the soldier
        originLocation = transform.position;

        // Set soldier parameters, depending on whether it is attacking or defending
        switch (role)
        {
            case Role.Attacker:
                normalSpeed = 1.5f * Time.deltaTime;
                carryingSpeed = 0.75f * Time.deltaTime;
                reactivateTime = 2.5f;
                break;

            case Role.Defender:
                normalSpeed = 1.0f * Time.deltaTime;
                returnSpeed = 2f * Time.deltaTime;
                reactivateTime = 4f;
                break;
        }

        StartCoroutine(OnSpawn(SPAWNTIME));
    }

    // Update is called once per frame
    void Update()
    {
        if (isactivated)
        {
            // Attacker behavior
            if (role == Role.Attacker)
            {
                // Chase the ball if it is not being held

                // When holding the ball

                // When there is no ball to chase or hold
                Move(normalSpeed);
            }
            else if (role == Role.Defender)
            {
                // On standby

                // Chase when the attacker with ball enters detection circle

            }
        }
    }

    void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed, Space.Self);
    }

    void Move(float speed, Vector3 destination)
    {

    }

    IEnumerator OnSpawn(float activationTime)
    {
        yield return new WaitForSeconds(activationTime);
        // Activate the soldier
        isactivated = true;
    }

    #endregion
}
