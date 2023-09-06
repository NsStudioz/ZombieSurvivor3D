using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    [SerializeField] Transform target = null;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform retreatTransform;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            target = playerTransform;
            navMeshAgent.destination = target.position;
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            target = retreatTransform;
            navMeshAgent.destination = target.position;
        }
    }
}
