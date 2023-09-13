using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{

    [SerializeField] int weaponDamage;

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            damageable?.TakeDamage(weaponDamage); // if damageable is not null...Then Damage
            Debug.Log("Enemy Hit!");
            Destroy(gameObject);
        }

        Destroy(gameObject);
        // Play effects on hit maybe... 
    }

}
