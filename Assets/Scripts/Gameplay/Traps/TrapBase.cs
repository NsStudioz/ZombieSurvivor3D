using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Traps
{
    public abstract class TrapBase : GameListener
    {

        [Header("Attributes")]
        [SerializeField] private int pointsCost;
        [SerializeField] protected bool isActivated = false;
        [SerializeField] protected int damage;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (isActivated)
                Deactivate();
        }

        public int GetPointCost()
        {
            return pointsCost;
        }

        public void Activate()
        {
            isActivated = true;
        }

        public void Deactivate()
        {
            isActivated = false;
        }


    }
}
