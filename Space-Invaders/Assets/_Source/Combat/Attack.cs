using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Combat
{
    public class Attack
    {
        private readonly float _shootingInterval;
        private readonly GameObject _bulletPrefab;
        private readonly Transform _attackerTransform;

        private bool _canShoot = true;
        
        public Attack(float shootingInterval, GameObject bulletPrefab, Transform attackerTransform)
        {
            _shootingInterval = shootingInterval;
            _bulletPrefab = bulletPrefab;
            _attackerTransform = attackerTransform;
        }
        
        public void Shoot()
        {
            if (_canShoot)
            {
                ShootWithIntervalAsync(CancellationToken.None).Forget();
            }
        }

        private async UniTask ShootWithIntervalAsync(CancellationToken token)
        {
            _canShoot = false;
            Object.Instantiate(_bulletPrefab, _attackerTransform.position, Quaternion.identity);
            await UniTask.Delay(TimeSpan.FromSeconds(_shootingInterval), cancellationToken: token);
            _canShoot = true;
        }
    }
}