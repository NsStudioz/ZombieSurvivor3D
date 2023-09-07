using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    
    private static GameStateManager instance;

    //Events:
    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    // Constructor:
    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameStateManager();

            return instance;
        }
    }

    public enum GameState
    {
        Gameplay,
        Paused
    }

    public GameState CurrentGameState { get; private set; }



    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }

}
