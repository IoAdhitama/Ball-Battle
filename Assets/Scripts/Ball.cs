using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.OnMatchStart += spawnBall;
    }

    private void spawnBall(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
