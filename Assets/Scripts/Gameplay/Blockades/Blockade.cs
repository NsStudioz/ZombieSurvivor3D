using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Blockades
{
    public class Blockade : MonoBehaviour
    {

        [Header("Attributes")]
        [SerializeField] private int pointsCost;

        #region EventListeners:

        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        #endregion

        public int GetPointCost()
        {
            return pointsCost;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
