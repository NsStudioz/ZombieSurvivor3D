using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Player
{
    public class PlayerTag : MonoBehaviour
    {
        public static PlayerTag Instance;

        public Transform playerTransform;

        private void Awake()
        {
            Instance = this;

            playerTransform = GetComponent<Transform>();
        }
    }
}

