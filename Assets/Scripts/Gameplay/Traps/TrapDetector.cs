using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZombieSurvivor3D.Gameplay.Score;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public class TrapDetector : GameListener, ControlsPC.IInteractionControlsActions
    {

        [Header("Attributes")]
        [SerializeField] private TrapBase TrapBaseObject = null;
        [SerializeField] private bool isPressed;

        private const string TRAP_TAG = "Traps";


        #region Collisions:

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(TRAP_TAG))
                TrapBaseObject = col.GetComponent<TrapBase>();
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag(TRAP_TAG))
                TrapBaseObject = null;
        }

        #endregion

        private void ActivateTrap()
        {
            ScoreSystem.Instance.UpdateScore(TrapBaseObject.GetPointCost());
            TrapBaseObject.Activate();
            TrapBaseObject = null;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (TrapBaseObject == null)
                return;

            if (context.performed)
                isPressed = true;

            else if (context.canceled)
                isPressed = false;

            if (isPressed)
            {
                ActivateTrap();
                isPressed = false;
            }
        }
    }
}
