using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Handheld;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class ArsenalBoxDetector : MonoBehaviour, ControlsPC.IInteractionControlsActions
    {

        [SerializeField] private static ArsenalBox ArsenalBoxObject = null;
        [SerializeField] private bool isPressed;

        private const string ARSENALBOX_TAG = "ArsenalBox";

        #region EventListeners:

        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            HandheldCarrier.OnArsenalBoxItemInteracted += ArsenalBoxTriggerRemoveLoot;  
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            HandheldCarrier.OnArsenalBoxItemInteracted -= ArsenalBoxTriggerRemoveLoot;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        // Invoker: HandheldCarrier:
        private void ArsenalBoxTriggerRemoveLoot()
        {
            if (GetArsenalBoxLootState())
                GetArsenalBox().RemoveLootFromBox();
            //Debug.Log("Loot Removed from: " + instance);
        }

        #endregion

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(ARSENALBOX_TAG))
                ArsenalBoxObject = col.GetComponent<ArsenalBox>();
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag(ARSENALBOX_TAG))
                ArsenalBoxObject = null;
        }

        public static bool GetArsenalBoxLootState()
        {
            return ArsenalBoxObject != null;
        }

        public static ArsenalBox GetArsenalBox()
        {
            return ArsenalBoxObject;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (ArsenalBoxObject == null)
                return;

            if (context.performed)
                isPressed = true;

            else if (context.canceled)
                isPressed = false;

            if (isPressed)
            {
                OpenInteractableArsenalBox();
                isPressed = false;
            }
        }

        private void OpenInteractableArsenalBox()
        {
            ScoreSystem.Instance.UpdateScore(ArsenalBoxObject.GetPointsCost());
            ArsenalBoxObject.OpenArsenalBoxForLoot();
        }
    }
}
