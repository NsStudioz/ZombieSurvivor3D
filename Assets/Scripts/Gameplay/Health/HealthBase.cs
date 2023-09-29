using System.Collections;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Health
{
    public class HealthBase : MonoBehaviour
    {

        [Header("Main Elements")]
        [SerializeField] protected HealthComponent healthComponent;
        protected const int ZERO_HEALTH = 0;

        protected void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        protected void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        protected void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        protected virtual void InitializeHealthComponent(int health)
        {
            HealthComponent newHealthComp = new HealthComponent(health);
            healthComponent = newHealthComp;
        }

        // MUST BE PUBLIC, IN ORDER TO ACCESS DAMAGE:
        public virtual void TakeDamage(int damageAmount)
        {
            healthComponent.TakeDamage(damageAmount);
        }
    }
}