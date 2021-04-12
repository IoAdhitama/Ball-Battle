using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PreGame,
        BlueAttack,
        RedAttack,
        PenaltyGame,
        GameEnd
    }

    public GameState currentGameState = GameState.PreGame;

    /*
     * Handle each states and changes, etc.
     * Handle the whole game, really lmao
     */
}
