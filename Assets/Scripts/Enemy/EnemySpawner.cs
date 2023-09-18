using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("Main Elements")]
    [SerializeField] List<GameObject> enemyStandardPrefabs = new List<GameObject>();
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawning")]
    [SerializeField] int pointerIndex = 0;
    [SerializeField] int intRandomizer;
    //
    [SerializeField] float timer;
    [SerializeField] float timerThreshold;

    [Header("Limits")]
    [SerializeField] int minInt;
    [SerializeField] int maxInt;
    [SerializeField] int minFloat;
    [SerializeField] int maxFloat;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
