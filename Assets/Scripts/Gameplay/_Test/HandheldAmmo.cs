using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class HandheldAmmo : MonoBehaviour
    {
        // Ammunition Counter per handheld/weapon:

        /// <summary>
        /// Return remaining ammo in magazine.
        /// </summary>
        public int CurrentMagAmmo { get; private set; }

        /// <summary>
        /// Return remaining reserved ammo.
        /// </summary>
        public int CurrentReservedAmmo { get; private set; }

        public void SetCurrentMagAmmo(int magAmmo)
        {
            CurrentMagAmmo = magAmmo;
        }

        public void SetCurrentReservedAmmo(int maxAmmo)
        {
             CurrentReservedAmmo = maxAmmo;
        }

        /*        [SerializeField] private int currentMagAmmo;
        [SerializeField] private int currentReservedAmmo;*/

    }
}
