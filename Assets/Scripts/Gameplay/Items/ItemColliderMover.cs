using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D.Gameplay.Items
{
    public class ItemColliderMover : GameListener
    {

        [Header("Main Attributes")]
        [SerializeField] private BoxCollider col;
        [SerializeField] private Vector3 pos;
        public bool isTriggered { get; private set; }

        [Header("Collider Speed")]
        [SerializeField] private int speed;

        [Header("Timer")]
        [SerializeField] private float timeElapsed;
        [SerializeField] private float timeElapsedThreshold = 1f;
        private float zeroValue = 0f;

        protected override void Awake()
        {
            base.Awake();
            //
            col = GetComponent<BoxCollider>();
            pos = new Vector3(zeroValue, pos.y + 10f, zeroValue);
            timeElapsed = timeElapsedThreshold;
        }

        void Update()
        {
            if (isTriggered && timeElapsed >= zeroValue)
            {
                timeElapsed -= Time.deltaTime;
                col.center += pos * Time.deltaTime * speed;
            }

            if (timeElapsed <= 0)
                Destroy(transform.parent.gameObject);
        }

        public void Move()
        {
            isTriggered = true;
        }
    }
}
