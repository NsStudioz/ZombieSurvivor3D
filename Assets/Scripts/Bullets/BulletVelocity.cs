using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVelocity : MonoBehaviour
{

    [SerializeField] private float bulletSpeed;


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
