using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerHealth: MonoBehaviour
    {
        public event Action<int> OnPlayerHealthChanged;
        
        [SerializeField] private int healthAmount;
        public void TakeDamage(int damage)
        {
            healthAmount -= damage;
            if (healthAmount < 0)
            {
                healthAmount = 0;
                //todo game over
            }
            OnPlayerHealthChanged?.Invoke(healthAmount);
        }
    }
}