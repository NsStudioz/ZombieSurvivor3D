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

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Start()
    {
        
    }


    void Update()
    {

    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }


    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        enabled = newGameState == GameStateManager.GameState.Gameplay;
    }

}
