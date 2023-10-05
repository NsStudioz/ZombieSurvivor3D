using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Score
{
    [System.Serializable]
    public struct ScoreComponent
    {

        public int HighScore;

        public ScoreComponent(int initValue)
        {
            HighScore = initValue;
        }

        public int GetUpdatedHighScore(int scorePoints)
        {
            return HighScore + scorePoints;
        }

        public void UpdateScore(int scorePoints)
        {
            HighScore = GetUpdatedHighScore(scorePoints);
        }
    }
}

