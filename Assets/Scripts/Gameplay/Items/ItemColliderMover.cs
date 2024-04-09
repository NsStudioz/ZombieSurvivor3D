using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Items
{
    public class ItemColliderMover : MonoBehaviour
    {

        [Header("Main Attributes")]
        [SerializeField] private BoxCollider col;
        [SerializeField] private Vector3 pos;
        public bool isTriggered { get; private set; }

        [Header("Collider Speed")]
        [SerializeField] private int speed;

        [Header("Timer")]
        [SerializeField] private float timeElapsed;
        [SerializeField] private float timeElapsedThreshold = 1f;
        private float zeroValue = 0f;

        void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
            //
            col = GetComponent<BoxCollider>();
            pos = new Vector3(zeroValue, pos.y + 10f, zeroValue);
            timeElapsed = timeElapsedThreshold;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

        void Update()
        {
            if (isTriggered && timeElapsed >= zeroValue)
            {
                timeElapsed -= Time.deltaTime;
                col.center += pos * Time.deltaTime * speed;
            }

            if (timeElapsed <= 0)
                Destroy(transform.parent.gameObject);
        }

        public void Move()
        {
            isTriggered = true;
        }
    }
}
