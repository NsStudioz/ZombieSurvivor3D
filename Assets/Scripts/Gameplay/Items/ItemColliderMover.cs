using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class ItemColliderMover : MonoBehaviour
    {

        public bool isTriggered { get; private set; }

        //[SerializeField] private bool isTriggeredTest;

        [SerializeField] private float timeElapsed;
        [SerializeField] private float timeElapsedThreshold = 1f;
        private float zeroValue = 0f;

        [SerializeField] private BoxCollider col;
        [SerializeField] private Vector3 pos;
        [SerializeField] private int speed;

        // Start is called before the first frame update
        void Awake()
        {
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
