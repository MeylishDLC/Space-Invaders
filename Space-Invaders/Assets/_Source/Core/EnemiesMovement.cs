using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class EnemiesMovement: MonoBehaviour
    {
        [Header("Screen")]
        [SerializeField] private float screenHorizontalPadding;
        [SerializeField] private Camera mainCamera;
        
        [Header("Movement Horizontal")] 
        [SerializeField] private float horizontalMoveValue; 
        [SerializeField] private float horizontalMoveSpeed;
        [SerializeField] private float moveSpeedIncrease;
        [SerializeField] private float enemyHorizontalMoveInterval;
        
        [Header("Movement Lower")]
        [SerializeField] private float moveLowerValue;
        [SerializeField] private float delayBetweenEnemyMoveLower;

        private bool _isMovingRight;
        private float _currentMoveSpeed;
        private List<GameObject> _allEnemies;
        private void OnValidate()
        {
            if (Camera.main != null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Start()
        {
            _currentMoveSpeed = horizontalMoveSpeed;
            MoveEnemiesHorizontallyCycle(CancellationToken.None).Forget();
            SubscribeOnEvents();
        }
        private async UniTask MoveEnemiesAsync(CancellationToken token)
        {
            _allEnemies = GetAllCurrentEnemies();
            _allEnemies.Reverse();
            _allEnemies.Remove(_allEnemies.ElementAt(_allEnemies.Count - 1));
            
            foreach (var enemy in _allEnemies.Where(enemy => enemy != null))
            {
                enemy.transform.position += new Vector3(0, -moveLowerValue, 0);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenEnemyMoveLower), cancellationToken: token);
            }
        }
        
        private async UniTask MoveEnemiesHorizontallyCycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_currentMoveSpeed), cancellationToken: token);
                await MoveEnemiesHorizontally(token);
            }
        }

        private async UniTask MoveEnemiesHorizontally(CancellationToken token)
        {
            _allEnemies = GetAllCurrentEnemies();
            var rightWallPos = mainCamera.orthographicSize * mainCamera.aspect;
            
            if (_isMovingRight)
            {
                var rightmostEnemy = _allEnemies.OrderByDescending(e => e.transform.position.x).FirstOrDefault();
                if (rightmostEnemy != null && rightmostEnemy.transform.position.x + horizontalMoveValue 
                    > rightWallPos - screenHorizontalPadding)
                {
                    _isMovingRight = false;
                    await MoveEnemiesAsync(token); 
                }
            }
            else
            {
                var leftmostEnemy = _allEnemies.OrderBy(e => e.transform.position.x).FirstOrDefault();
                if (leftmostEnemy != null && leftmostEnemy.transform.position.x - horizontalMoveValue 
                    < -rightWallPos + screenHorizontalPadding)
                {
                    _isMovingRight = true;
                    await MoveEnemiesAsync(token); 
                }
            }

            foreach (var enemy in _allEnemies.Where(enemy => enemy != null))
            {
                enemy.transform.position += new Vector3(_isMovingRight ? horizontalMoveValue : -horizontalMoveValue, 0, 0);
                await UniTask.Delay(TimeSpan.FromSeconds(enemyHorizontalMoveInterval), cancellationToken: token);
            }
        }
        private List<GameObject> GetAllCurrentEnemies()
        {
            var allEnemies = GetComponentsInChildren<EnemyController>();
            
            var enemiesTransforms = allEnemies.Select(enemy => enemy.GetComponent<Transform>()).ToList();

            var childrenList = enemiesTransforms.Select(child => child.gameObject).ToList();
            return childrenList;
        }

        private void SpeedUpEnemies(EnemyController deadEnemy)
        {
            deadEnemy.OnEnemyDeath -= SpeedUpEnemies;
            
            _currentMoveSpeed -= moveSpeedIncrease;
            if (_currentMoveSpeed < 0)
            {
                _currentMoveSpeed = 0.0001f;
            }
        }
        
        private void SubscribeOnEvents()
        {
            var enemyObjects = GetAllCurrentEnemies();
            foreach (var enemy in enemyObjects)
            {
                var enemyController =  enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.OnEnemyDeath += SpeedUpEnemies;
                }
            }
        }
    }
}