using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D.Gameplay.Blockades
{
    public class BlockadeDetector : MonoBehaviour, ControlsPC.IInteractionControlsActions
    {

        [Header("Attributes")]
        [SerializeField] private Blockade BlockadeObject = null;
        [SerializeField] private bool isPressed;

        private const string BLOCKADE_TAG = "Blockade";

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

        #region Collisions: 

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(BLOCKADE_TAG))
                BlockadeObject = col.GetComponent<Blockade>();
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag(BLOCKADE_TAG))
                BlockadeObject = null;
        }

        #endregion

        #region Input:

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (BlockadeObject == null)
                return;

            if (context.performed)
                isPressed = true;

            else if (context.canceled)
                isPressed = false;

            if (isPressed)
            {
                OpenBlockade();
                isPressed = false;
            }
        }

        private void OpenBlockade()
        {
            ScoreSystem.Instance.UpdateScore(BlockadeObject.GetPointCost());
            BlockadeObject.Destroy();
            BlockadeObject = null;
        }

        #endregion
    }
}
