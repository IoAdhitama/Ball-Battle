using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask layerMask;

    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject soldierPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get input, check whether it is a valid location for spawning soldiers
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            Debug.Log(raycastHit);
        }

        // If it is, spawn a soldier based on the current game state
        Instantiate(soldierPrefab, raycastHit.point, Quaternion.identity);
    }
}
