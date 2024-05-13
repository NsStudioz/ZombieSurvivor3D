using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZombieSurvivor3D.Gameplay.Bullets
{
    public class BulletVelocity : GameListener
    {

        [Header("Attributes")]
        [SerializeField] float bulletSpeed;

        // Future Uses
        /*  [SerializeField] float _UpwardForce;
            [SerializeField] float _BulletDownwardForce;
            [SerializeField] float _BulletDownwardForceThreshold;*/

        void Update()
        {
            SetBulletVelocityForward(Time.deltaTime);
        }

        private void SetBulletVelocityForward(float deltaTime)
        {
            float speed = bulletSpeed * deltaTime;
            transform.Translate(transform.forward * speed, Space.World);
        }
    }
}
