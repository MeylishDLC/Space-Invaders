using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerHealth: MonoBehaviour
    {
        public event Action<int> OnPlayerHealthChanged;
        
        [SerializeField] private int healthAmount;
        
        [Header("Invincibility Display")]
        [SerializeField] private float invincibilityDuration;
        [SerializeField] private float blinkDuration;

        private bool _canTakeDamage = true;
        private SpriteRenderer _spriteRenderer;
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
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
        public int GetPlayerHealth()
        {
            return healthAmount;
        }
        private async UniTask BeInvincible(CancellationToken token)
        {
            _canTakeDamage = false;

            var loops = Convert.ToInt32(invincibilityDuration / blinkDuration);
            await _spriteRenderer.DOFade(0, blinkDuration).SetLoops(loops).ToUniTask(cancellationToken: token);
            
            var color = _spriteRenderer.color;
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1);

            _canTakeDamage = true;
        }
        
    }
}