﻿using System.Collections;
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
        Debug.Log("Aggro circle collided with " + other);
        if (other.GetComponentInParent<Soldier>().isHoldingBall) // Attacker with the ball is found!
        {
            Debug.Log("Aggro circle collided with an attacker carrying a ball!");
            SetTarget(other.gameObject);
        }
        else return;
    }

    private void SetTarget(GameObject target)
    {
        soldier.SetTarget(target);
    }
}