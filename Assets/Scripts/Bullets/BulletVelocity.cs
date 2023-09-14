using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVelocity : MonoBehaviour
{

    [SerializeField] private float bulletSpeed;

    // Future Uses
/*    [SerializeField] private float _UpwardForce;
    [SerializeField] private float _BulletDownwardForce;
    [SerializeField] private float _BulletDownwardForceThreshold;*/


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
