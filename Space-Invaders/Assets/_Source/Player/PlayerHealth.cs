using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sound;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

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
        private SoundManager _soundManager;
        private FMODEvents _fmodEvents;
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        [Inject]
        public void Initialize(SoundManager soundManager, FMODEvents fmodEvents)
        {
            _soundManager = soundManager;
            _fmodEvents = fmodEvents;
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
            
            OnPlayerHealthChanged?.Invoke(healthAmount);
            _soundManager.PlayOneShot(_fmodEvents.hitPlayerSound);
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