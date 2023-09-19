using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
