using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public abstract class TrapBase : MonoBehaviour
    {

        [SerializeField] private int pointsCost;
        [SerializeField] protected bool isActivated = false;

        protected virtual void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        protected virtual void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;


            if (isActivated)
                Deactivate();
        }

        public int GetPointCost()
        {
            return pointsCost;
        }

        public virtual void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            isActivated = newGameState == GameStateManager.GameState.Gameplay;
            //enabled = newGameState == GameStateManager.GameState.Gameplay;
            //base.OnGameStateChanged(newGameState);  // for child, if needed.
        }

        public void Activate()
        {
            isActivated = true;
        }

        public void Deactivate()
        {
            isActivated = false;
        }


    }
}