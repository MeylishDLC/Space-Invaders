using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Combat
{
    public class Attack
    {
        public bool CanShoot { get; protected set; } = true;
        
        protected readonly GameObject _bulletPrefab;
        protected readonly Transform _attackerTransform;
        private readonly float _shootingInterval;
        public Attack(float shootingInterval, GameObject bulletPrefab, Transform attackerTransform)
        {
            _shootingInterval = shootingInterval;
            _bulletPrefab = bulletPrefab;
            _attackerTransform = attackerTransform;
        }
        
        public void Shoot()
        {
            if (CanShoot)
            {
                ShootWithIntervalAsync(CancellationToken.None).Forget();
            }
        }

        protected virtual async UniTask ShootWithIntervalAsync(CancellationToken token)
        {
            CanShoot = false;
            
            if (_attackerTransform != null)
            {
                Object.Instantiate(_bulletPrefab, _attackerTransform.position, Quaternion.identity);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_shootingInterval), cancellationToken: token);
            CanShoot = true;
        }
    }
}