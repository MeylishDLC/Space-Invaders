using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Combat
{
    public class AttackWithRandomDelay: Attack
    {
        private readonly float _minShootingInterval;
        private readonly float _maxShootingInterval;

        public AttackWithRandomDelay(float minShootingInterval, float maxShootingInterval, GameObject bulletPrefab, Transform attackerTransform)
            : base(minShootingInterval, bulletPrefab, attackerTransform)
        {
            _minShootingInterval = minShootingInterval;
            _maxShootingInterval = maxShootingInterval;
        }

        protected override async UniTask ShootWithIntervalAsync(CancellationToken token)
        {
            _canShoot = false;
            
            if (_attackerTransform != null)
            {
                Object.Instantiate(_bulletPrefab, _attackerTransform.position, Quaternion.identity);
            }

            var randomInterval = Random.Range(_minShootingInterval, _maxShootingInterval);
            
            await UniTask.Delay(TimeSpan.FromSeconds(randomInterval), cancellationToken: token);
            _canShoot = true;
        }
    }
}