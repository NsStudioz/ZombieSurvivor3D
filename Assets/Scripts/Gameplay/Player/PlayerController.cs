using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Elements")]
        Camera mainCamera;

        [Header("RayCast Elements")]
        [SerializeField] LayerMask rayCastHitLayers;
        [SerializeField] float rayMaxDistance;
        Ray cameraRay;
        RaycastHit cameraRayHit;

        [Header("Main Elements")]
        [SerializeField] Vector3 forward, right;
        [SerializeField] float moveSpeed = 1f;
        private const float zeroFloat = 0f;

        #region EventListeners:

        private void Awake()
        {
            mainCamera = Camera.main;
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

        void Start()
        {
            InitMovement();
        }

        void Update()
        {
            MovePlayer(Time.deltaTime);
            Look();
        }

        #region Move & Look:

        private void InitMovement()
        {
            forward = mainCamera.transform.forward;
            forward.y = 0f;
            forward = Vector3.Normalize(forward);

            right = Quaternion.Euler(new Vector3(zeroFloat, 90, zeroFloat)) * forward;
        }

        private void MovePlayer(float deltaTime)
        {
            Vector3 rightMovement = right * moveSpeed * deltaTime * Input.GetAxis("Horizontal");
            Vector3 upMovement = forward * moveSpeed * deltaTime * Input.GetAxis("Vertical");

            Vector3.Normalize(rightMovement);
            Vector3.Normalize(upMovement);

            transform.position += rightMovement;
            transform.position += upMovement;
        }

        private void Look() // using raycast
        {
            cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(cameraRay, out cameraRayHit, rayMaxDistance, rayCastHitLayers))
            {
                Vector3 targetPos = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                transform.LookAt(targetPos);
            }
        }

        #endregion
    }
}
