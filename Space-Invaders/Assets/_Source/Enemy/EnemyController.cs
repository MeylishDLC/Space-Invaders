using System;
using System.Threading;
using Combat;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyController: MonoBehaviour
    {
        [SerializeField] private float shootingInterval;
        [SerializeField] private EnemyBullet bulletPrefab;
        
        private Attack _enemyAttack;

        private void Start()
        {
            _enemyAttack = new Attack(shootingInterval, bulletPrefab.gameObject, gameObject.transform);
            StartAttackLoop(CancellationToken.None).Forget();
        }

        private async UniTask StartAttackLoop(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(shootingInterval), cancellationToken: token);
            
            while (gameObject != null)
            {
                _enemyAttack.Shoot();
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}