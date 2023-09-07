using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;

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




}
