using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace ZombieSurvivor3D.Blockades
{
    public class Blockade : MonoBehaviour
    {

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
