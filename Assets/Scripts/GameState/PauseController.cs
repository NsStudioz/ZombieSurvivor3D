using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.GameState currentGameState = GameStateManager.Instance.CurrentGameState;

            // Ternary operator => is the new state = Gameplay ? true : false;
            GameStateManager.GameState newGameState = currentGameState == GameStateManager.GameState.Gameplay ?
                GameStateManager.GameState.Paused : 
                GameStateManager.GameState.Gameplay; 

            GameStateManager.Instance.SetState(newGameState);

            Debug.Log("GameState Changed: " + newGameState);
        }
    }
}
