using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{
    float aggroRange;
    [SerializeField] Soldier soldier;

    // Start is called before the first frame update
    void Start()
    {
        aggroRange = 30 * 0.35f;
        transform.localScale = new Vector3(aggroRange, aggroRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) // Attacker with the ball is found!
        {
            SetTarget(other.gameObject);
        }
        else return;
    }

    private void SetTarget(GameObject target)
    {
        if (target != null)
        {
            soldier.SetTarget(target);
        }
    }
}
