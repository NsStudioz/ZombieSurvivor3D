using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;


namespace ZombieSurvivor3D.Gameplay.Score
{
    public class ScoreSystem : MonoBehaviour
    {
        public static ScoreSystem Instance;


        [SerializeField] ScoreComponent scoreComponent;
        [SerializeField] int highScoreForUI;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Something went wrong. there are more than 1 ScoreSystems in the game!");
                return;
            }
            Instance = this;

            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void Start()
        {
            InitializeScoreComponent();
        }

        private void InitializeScoreComponent()
        {
            ScoreComponent newScoreComponent = new ScoreComponent(0);
            scoreComponent = newScoreComponent;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        public void UpdateScore(int scorePoints)
        {
            scoreComponent.highScore = scoreComponent.GetUpdatedHighScore(scorePoints);
            highScoreForUI = scoreComponent.highScore;
        }


        private void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            enabled = newGameState == GameStateManager.GameState.Gameplay;
        }
    }
}
