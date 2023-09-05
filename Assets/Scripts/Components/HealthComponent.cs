using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HealthComponent
{
    public int currentHealth;
    public int maxHealth;

    public HealthComponent(int _maxHealth)
    {
        maxHealth = _maxHealth;
        currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public int GetCurrentHealth() 
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetCurrentHealthToMax()
    {
        currentHealth = maxHealth;
    }

}
