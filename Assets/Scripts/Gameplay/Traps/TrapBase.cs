using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D
{
    public abstract class TrapBase : MonoBehaviour
    {

        [SerializeField] protected bool isActivated = false;

        protected virtual void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        protected virtual void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;


            /*            if (isActivated)
                            Deactivate();*/
        }

        public virtual void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            isActivated = newGameState == GameStateManager.GameState.Gameplay;
            //enabled = newGameState == GameStateManager.GameState.Gameplay;
            //base.OnGameStateChanged(newGameState);  // for child, if needed.
        }

        protected void Activate()
        {
            isActivated = true;
        }

        protected void Deactivate()
        {
            isActivated = false;
        }


    }
}
