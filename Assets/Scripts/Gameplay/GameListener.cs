using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D
{
    public class GameListener : MonoBehaviour
    {
        protected virtual void Awake()
        {
            //GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            EventManager<GameStateManager.GameState>.Register(
                Events.EventKey.OnGameStateChange.ToString(), 
                OnGameStateChanged);
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
        protected virtual void OnDestroy()
        {
            //GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            EventManager<GameStateManager.GameState>.Unregister(
                Events.EventKey.OnGameStateChange.ToString(), 
                OnGameStateChanged);
        }

        protected virtual void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

    }
}
