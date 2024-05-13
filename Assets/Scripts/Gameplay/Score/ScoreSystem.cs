using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Score
{
    public class ScoreSystem : GameListener
    {

        public static ScoreSystem Instance;

        [Header("Attributes")]
        [SerializeField] ScoreComponent scoreComponent;
        [SerializeField] int highScoreForUI;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Something went wrong. there are more than 1 ScoreSystems in the game!");
                return;
            }
            Instance = this;

            base.Awake();
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

        public void UpdateScore(int scorePoints)
        {
            scoreComponent.HighScore = scoreComponent.GetUpdatedHighScore(scorePoints);
            highScoreForUI = scoreComponent.HighScore;
        }
    }
}
