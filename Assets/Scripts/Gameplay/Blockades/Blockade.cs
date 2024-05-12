using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Blockades
{
    public class Blockade : GameListener
    {

        [Header("Attributes")]
        [SerializeField] private int pointsCost;

        public int GetPointCost()
        {
            return pointsCost;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
