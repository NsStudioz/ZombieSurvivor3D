using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D.Gameplay.Bullets
{
    public class BulletVelocity : MonoBehaviour
    {

        [SerializeField] private float bulletSpeed;

        // Future Uses
        /*    [SerializeField] private float _UpwardForce;
            [SerializeField] private float _BulletDownwardForce;
            [SerializeField] private float _BulletDownwardForceThreshold;*/

        private void Start()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        void Update()
        {
            SetBulletVelocityForward(Time.deltaTime);
        }

        private void SetBulletVelocityForward(float deltaTime)
        {
            float speed = bulletSpeed * deltaTime;
            transform.Translate(transform.forward * speed, Space.World);
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }

    }
}
