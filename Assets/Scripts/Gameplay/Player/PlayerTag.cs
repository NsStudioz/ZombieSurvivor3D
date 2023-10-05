using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Player
{
    public class PlayerTag : MonoBehaviour
    {
        public static PlayerTag Instance;

        public Transform PlayerTransform;

        private void Awake()
        {
            Instance = this;

            PlayerTransform = GetComponent<Transform>();
        }
    }
}

