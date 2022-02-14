using System;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class HealthHolder : IHealthHolder
    {
        #region Fields

        private int _maxHealth;
        private int _currentHealth;

        public event Action Death;
        public event Action Damaged;

        private readonly SubscriptionProperty<float> _healthPercentage;

        #endregion


        #region Properties

        public int CurrentHelth => _currentHealth;

        public bool IsDead => _currentHealth <= 0;

        private float HelthPerc => (float)_currentHealth / (float)_maxHealth;

        public SubscriptionProperty<float> HealthPercentage => _healthPercentage;

        #endregion


        #region ClassLifeCycles

        public HealthHolder(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            _healthPercentage = new SubscriptionProperty<float>();

            _healthPercentage.Value = HelthPerc;
        }

        #endregion


        #region IHealthHolder

        public void GetHealth(int amount)
        {
            _currentHealth = Mathf.Max(_currentHealth + amount, _maxHealth);
        }

        public void ResetHealth(int newMaxAmount)
        {
            _maxHealth = newMaxAmount;
            _currentHealth = newMaxAmount;

            HealthPercentage.Value = HelthPerc;
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;

            HealthPercentage.Value = HelthPerc;
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;

            _currentHealth -= damage;
            
            HealthPercentage.Value = HelthPerc;

            if (IsDead)
                Death?.Invoke();
            else
                Damaged?.Invoke();

        }

        #endregion
    }
}