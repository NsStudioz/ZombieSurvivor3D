using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Main Elements")]
    [SerializeField] HealthComponent healthComponent;

    [Header("Player Elements")]
    [SerializeField] bool isPlayerHit = false;
    [SerializeField] float timeElapsed;
    [SerializeField] float timeElapsedThreshold = 3f;

    private void Start()
    {
        HealthComponent healthComponentInstance = new HealthComponent(3);
        healthComponent = healthComponentInstance;
    }

    private void Update()
    {
        TakeDamageTest();
        RegisterPlayerHit();
        RegenerateHealth();
    }

    private void TakeDamageTest()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            healthComponent.TakeDamage(1);
            Debug.Log("Player Health: " + healthComponent.GetCurrentHealth());
            timeElapsed = timeElapsedThreshold;
        }

        if (healthComponent.GetCurrentHealth() <= 0)
            Debug.Log("Player: Died!");
    }

    private void RegisterPlayerHit()
    {
        if (healthComponent.GetCurrentHealth() < healthComponent.GetMaxHealth())
            isPlayerHit = true;
    }

    private void RegenerateHealth()
    {
        if (healthComponent.GetCurrentHealth() <= 0)
            return;

        if (isPlayerHit)
            timeElapsed -= Time.deltaTime;

        if (timeElapsed <= 0)
        {
            healthComponent.SetCurrentHealthToMax();
            isPlayerHit = false;
        }
    }

}
