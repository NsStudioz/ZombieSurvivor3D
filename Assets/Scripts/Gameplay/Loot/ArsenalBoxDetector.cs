using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.Handheld;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D.Gameplay.Loot
{
    public class ArsenalBoxDetector : GameListener, ControlsPC.IInteractionControlsActions
    {

        [Header("Attributes")]
        [SerializeField] private static ArsenalBox ArsenalBoxObject = null;
        [SerializeField] private bool isPressed;

        private const string ARSENALBOX_TAG = "ArsenalBox";

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
            HandheldCarrier.OnArsenalBoxItemInteracted += ArsenalBoxTriggerRemoveLoot;  
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandheldCarrier.OnArsenalBoxItemInteracted -= ArsenalBoxTriggerRemoveLoot;
        }

        #endregion

        private void ArsenalBoxTriggerRemoveLoot()
        {
            if (GetArsenalBoxLootState())
                GetArsenalBox().RemoveLootFromBox();
        }

        public static ArsenalBox GetArsenalBox()
        {
            return ArsenalBoxObject;
        }

        public static bool GetArsenalBoxLootState()
        {
            return ArsenalBoxObject != null;
        }
        
        #region Collisions:

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

        #endregion

        #region Input:

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

        #endregion


    }
}
