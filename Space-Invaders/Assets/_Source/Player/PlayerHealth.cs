using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerHealth: MonoBehaviour
    {
        public event Action<int> OnPlayerHealthChanged;
        
        [SerializeField] private int healthAmount;
        [SerializeField] private float invincibilityDuration;

        private bool _canTakeDamage = true;
        public void TakeDamage(int damage)
        {
            if (!_canTakeDamage)
            {
                return;
            }
            
            healthAmount -= damage;
            if (healthAmount < 0)
            {
                healthAmount = 0;
            }
            
            Debug.Log($"Player health: {healthAmount}");
            OnPlayerHealthChanged?.Invoke(healthAmount);
            BeInvincible(CancellationToken.None).Forget();
        }
        private async UniTask BeInvincible(CancellationToken token)
        {
            _canTakeDamage = false;
            await UniTask.Delay(TimeSpan.FromSeconds(invincibilityDuration), cancellationToken: token);
            _canTakeDamage = true;
        }
        public int GetPlayerHealth()
        {
            return healthAmount;
        }
    }
}