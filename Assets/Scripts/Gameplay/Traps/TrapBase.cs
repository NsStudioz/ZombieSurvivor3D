using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public abstract class TrapBase : MonoBehaviour
    {

        [Header("Init Attributes")]
        [SerializeField] private int pointsCost;
        [SerializeField] protected bool isActivated = false;
        [SerializeField] protected int damage;

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
            enabled = newGameState == GameStateManager.GameState.Gameplay;
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
