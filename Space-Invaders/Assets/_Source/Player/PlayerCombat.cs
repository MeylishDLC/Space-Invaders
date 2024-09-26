using System;
using System.Threading;
using Combat;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public class PlayerCombat
    {
        private readonly float _shootingInterval;
        private readonly PlayerBullet _bulletPrefab;
        private readonly Transform _playerTransform;

        private bool _canShoot = true;
        public PlayerCombat(float shootingInterval, PlayerBullet bulletPrefab, Transform playerTransform)
        {
            _shootingInterval = shootingInterval;
            _bulletPrefab = bulletPrefab;
            _playerTransform = playerTransform;
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
            Object.Instantiate(_bulletPrefab, _playerTransform.position, Quaternion.identity);
            await UniTask.Delay(TimeSpan.FromSeconds(_shootingInterval), cancellationToken: token);
            _canShoot = true;
        }
    }
}