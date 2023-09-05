using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Elements")]
    Camera mainCamera = Camera.main;

    [Header("Main Elements")]
    [SerializeField] float moveSpeed = 1f;
    private const float zeroFloat = 0f;
    [SerializeField] Vector3 forward, right;

    void Start()
    {
        forward = mainCamera.transform.forward;
        forward.y = 0f;
        forward = Vector3.Normalize(forward);
    }


    void Update()
    {
        MovePlayer(Time.deltaTime);
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
    

}
