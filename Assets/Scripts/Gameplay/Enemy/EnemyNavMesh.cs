using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZombieSurvivor3D.Gameplay.GameState;
using ZombieSurvivor3D.Gameplay.Player;


namespace ZombieSurvivor3D.Gameplay.Enemy
{
    public class EnemyNavMesh : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;
        [SerializeField] Transform target = null;
        //[SerializeField] Transform playerTransform;
        [SerializeField] Transform retreatTransform;

        private void Awake()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            // prevents memory leaks and errors after the object is destroyed.
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            target = PlayerTag.Instance.PlayerTransform;
        }

        void Update()
        {
            // Works fine with GameState:
            if (target != null)
            {
                //target = playerTransform;
                navMeshAgent.destination = target.position;
            }

            //-------------------------------------------

            if (Input.GetKeyDown(KeyCode.B))
            {
                //target = playerTransform;
                navMeshAgent.destination = target.position;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                target = retreatTransform;
                navMeshAgent.destination = target.position;
            }
        }

        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
            navMeshAgent.enabled = newGameState == GameStateManager.GameState.Gameplay;
        }
    }
}
