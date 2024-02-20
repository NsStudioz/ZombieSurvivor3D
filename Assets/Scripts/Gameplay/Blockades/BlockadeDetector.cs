using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D.Gameplay.Blockades
{
    public class BlockadeDetector : MonoBehaviour, ControlsPC.IInteractionControlsActions
    {

        [SerializeField] private Blockade BlockadeObject = null;
        [SerializeField] private bool isPressed;

        private const string BLOCKADE_TAG = "Blockade";

<<<<<<< HEAD
        #region Event Listeners:
=======
        #region EventListeners:
>>>>>>> 11_RNGSystem

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
<<<<<<< HEAD
=======

>>>>>>> 11_RNGSystem

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

        private void OpenBlockade()
        {
            ScoreSystem.Instance.UpdateScore(BlockadeObject.GetPointCost());
            BlockadeObject.Destroy();
            BlockadeObject = null;
        }

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
    }
}
