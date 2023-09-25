using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScoreComponent 
{

    public int highScore;


    public int GetUpdatedHighScore(int scorePoints)
    {
        return highScore + scorePoints;
    }

}
