using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Score
{
    [System.Serializable]
    public struct ScoreComponent
    {

        public int highScore;

        public ScoreComponent(int initValue)
        {
            highScore = initValue;
        }

        public int GetUpdatedHighScore(int scorePoints)
        {
            return highScore + scorePoints;
        }

        public void UpdateScore(int scorePoints)
        {
            highScore = GetUpdatedHighScore(scorePoints);
        }
    }
}

