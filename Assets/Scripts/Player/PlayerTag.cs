using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    public static PlayerTag Instance;

    public Vector3 playerPos = Vector3.zero;

    public Transform playerTransform;

    private void Awake()
    {
        Instance = this;

        playerTransform = GetComponent<Transform>();
    }
}
