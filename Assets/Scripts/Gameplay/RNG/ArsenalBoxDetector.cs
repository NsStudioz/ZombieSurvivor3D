using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Blockades;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D
{
    public class ArsenalBoxDetector : MonoBehaviour, ControlsPC.IInteractionControlsActions
    {

        [SerializeField] private ArsenalBox ArsenalBoxObject = null;
        [SerializeField] private bool isPressed;

        private const string ARSENALBOX_TAG = "ArsenalBox";

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
