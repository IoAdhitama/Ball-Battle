using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region References
    [SerializeField] GameObject preMatchUI;
    [SerializeField] Text matchTimerText;
    [SerializeField] Button matchStartButton;
    [SerializeField] Text matchCountText;
    [SerializeField] Text matchScoreText;
    #endregion

    public enum GameState
    {
        PreGame, // Preparing for match or next round
        BlueAttack, // Blue/Player's turn to attack
        RedAttack, // Red/Enemy's turn to attack
        PenaltyGame, // On penalty game
        GameEnd // The game ends
    }

    public enum EndOfMatchReason
    {
        BallInGate, // Ball reaches opponent's gate, attacker win
        Timeout, // Time out, ball not in opponent gate, match draw
        AttackerOut // Attacker caught, no other attacker to pass ball to, defender win
    }

    public event EventHandler OnMatchStart;
    public event EventHandler OnMatchEnd;

    private GameState currentGameState = GameState.PreGame;

    // Match related infos
    int matchCount = 0;
    int blueWinCount = 0;
    int redWinCount = 0;

    public GameState GetGameState()
    {
        return currentGameState;
    }

    private void Start()
    {
        // After game is booted up, before the first match begins...
        // Initialize variables
        currentGameState = GameState.PreGame;
        Time.timeScale = 0f;
        matchCount = 1;

        // Subscribe to events
        OnMatchStart += HandleMatchStart;

        PreMatch();
    }

    private void PreMatch()
    {
        // Update pre match info
        SetPreMatchMenuInfo(matchCount, redWinCount, blueWinCount);
        preMatchUI.SetActive(true);
    }

    // Sets the UI text during prematch
    private void SetPreMatchMenuInfo(int matchCount, int redWins, int blueWins)
    {
        matchCountText.text = "Match " + matchCount;
        matchScoreText.text = blueWins + " - " + redWins;
    }

    public void OnMatchStartButtonClicked()
    {
        OnMatchStart?.Invoke(this, EventArgs.Empty);
    }

    private void HandleMatchStart(object sender, EventArgs e)
    {
        Debug.Log("Match is starting");
        Time.timeScale = 1f;
        // Disable the pre match UI
        preMatchUI.SetActive(false);

        // If match number is odd, blue (player) attacks
        if (matchCount % 2 == 1)
        { 
            currentGameState = GameState.BlueAttack; 
        }
        else
        {
            currentGameState = GameState.RedAttack;
        }

        // Start the match timer
        StartCoroutine(MatchTimeCountdown(140));
    }

    IEnumerator MatchTimeCountdown(int matchTime)
    {
        while (currentGameState != GameState.PreGame || currentGameState != GameState.GameEnd)
        {
            yield return new WaitForSeconds(1);
            matchTime -= 1;
            matchTimerText.text = matchTime + "s";

            if (matchTime == 0)
            {
                EndOfMatch(EndOfMatchReason.Timeout);
            }
        }
    }

    public void EndOfMatch(EndOfMatchReason reason)
    {
        Debug.Log("Match ending. Reason: " + reason);

        OnMatchEnd?.Invoke(this, EventArgs.Empty);

        // Stops the simulation
        Time.timeScale = 0f;

        // Process the reason for end of match
        switch (reason)
        {
            case EndOfMatchReason.BallInGate: // Ball in opponent's gate, attacker win
                if (currentGameState == GameState.BlueAttack)
                {
                    blueWinCount++;
                }
                else
                {
                    if (currentGameState == GameState.RedAttack)
                    {
                        redWinCount++;
                    }
                }
                break;

            case EndOfMatchReason.Timeout: // Timeout, ball not in gate, match draw
                break;

            case EndOfMatchReason.AttackerOut: // Attacker caught, no one to pass the ball to, defender win
                if (currentGameState == GameState.BlueAttack)
                {
                    redWinCount++;
                }
                else
                {
                    if (currentGameState == GameState.RedAttack)
                    {
                        blueWinCount++;
                    }
                }
                break;

            default:
                break;
        }

        // Other stuffs that happens on end of match
        matchCount++;

        // Revert to prematch state
        currentGameState = GameState.PreGame;
        PreMatch();
    }
}
