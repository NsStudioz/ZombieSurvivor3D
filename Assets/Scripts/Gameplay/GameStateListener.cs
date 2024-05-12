using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D
{
    public class GameStateListener : MonoBehaviour
    {

        protected void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }
/*        protected void Start()
        {
        
        }
        protected void OnEnable()
        {
            
        }
        protected void OnDisable()
        {
            
        }*/
        protected void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        protected virtual void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }


    }
}
