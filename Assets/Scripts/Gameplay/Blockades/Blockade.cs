using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Blockades
{
    public class Blockade : MonoBehaviour
    {

        [SerializeField] private int pointsCost;

        public void OpenBlockade(int pointsTotal)
        {
            pointsTotal -= pointsCost;
            Destroy(gameObject);
        }
    }
}
