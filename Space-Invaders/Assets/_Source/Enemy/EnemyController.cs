using System;
using Combat;
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
        }

        private void Update()
        {
            _enemyAttack.Shoot();
        }
    }
}