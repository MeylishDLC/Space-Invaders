using System;
using System.Threading;
using Combat;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController: MonoBehaviour
    {
        public event Action<EnemyController> OnEnemyDeath;
        [field:SerializeField] public int ScoreAmount { get; private set; }
        
        [SerializeField] private float minShootingInterval;
        [SerializeField] private float maxShootingInterval;
        [SerializeField] private EnemyBullet bulletPrefab;
        [SerializeField] private float timeBeforeDie;

        private Animator _animator;
        private Attack _enemyAttack;
        private static readonly int hit = Animator.StringToHash("hit");
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _enemyAttack = new AttackWithRandomDelay(minShootingInterval, maxShootingInterval, 
                bulletPrefab.gameObject, gameObject.transform);
            
            StartAttackLoop(CancellationToken.None).Forget();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")
                || other.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(9999);
            }
        }
        public void Die()
        {
            DieAsync(CancellationToken.None).Forget();
        }

        private async UniTask DieAsync(CancellationToken token)
        {
            OnEnemyDeath?.Invoke(this);
            
            _animator.SetTrigger(hit);
            await UniTask.Delay(TimeSpan.FromSeconds(timeBeforeDie), cancellationToken: token);
            Destroy(gameObject);
        }
        private async UniTask StartAttackLoop(CancellationToken token)
        {
            var randomInterval = Random.Range(2, maxShootingInterval);
            await UniTask.Delay(TimeSpan.FromSeconds(randomInterval), cancellationToken: token);
            
            while (this != null)
            {
                _enemyAttack.Shoot();
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}