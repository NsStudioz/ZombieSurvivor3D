namespace ZombieSurvivor3D.Gameplay.Health
{
    [System.Serializable]
    public struct HealthComponent
    {
        public int CurrentHealth;
        public int MaxHealth;

        public HealthComponent(int _maxHealth)
        {
            MaxHealth = _maxHealth;
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public int GetCurrentHealth()
        {
            return CurrentHealth;
        }

        public int GetMaxHealth()
        {
            return MaxHealth;
        }

        public void SetCurrentHealthToMax()
        {
            CurrentHealth = MaxHealth;
        }
    }
}

