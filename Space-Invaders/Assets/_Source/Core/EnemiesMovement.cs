﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class EnemiesMovement: MonoBehaviour
    {
        
        [Header("Screen Padding")]
        [SerializeField] private float screenHorizontalPadding;
        
        [Header("Movement Horizontal")] 
        [SerializeField] private float horizontalMoveValue;
        [SerializeField] private float horizontalMoveInterval;
        [SerializeField] private float enemyHorizontalMoveInterval;
        
        [Header("Movement Lower")]
        [SerializeField] private float moveLowerValue;
        [SerializeField] private float delayBetweenEnemyMoveLower;

        private bool _isMovingRight;
        private void Start()
        {
            MoveEnemiesHorizontallyCycle(CancellationToken.None).Forget();
        }
        private async UniTask MoveEnemiesAsync()
        {
            var allEnemies = GetAllCurrentEnemies();
            allEnemies.Reverse();
            allEnemies.Remove(allEnemies.ElementAt(allEnemies.Count - 1));
            
            foreach (var enemy in allEnemies.Where(enemy => enemy != null))
            {
                enemy.transform.position += new Vector3(0, -moveLowerValue, 0);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenEnemyMoveLower));
            }
        }
        
        private async UniTask MoveEnemiesHorizontallyCycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(horizontalMoveInterval), cancellationToken: token);
                await MoveEnemiesHorizontally(token);
            }
        }

        private async UniTask MoveEnemiesHorizontally(CancellationToken token)
        {
            var allEnemies = GetAllCurrentEnemies();
            var rightWallPos = Camera.main.orthographicSize * Camera.main.aspect;
            
            if (_isMovingRight)
            {
                var rightmostEnemy = allEnemies.OrderByDescending(e => e.transform.position.x).FirstOrDefault();
                if (rightmostEnemy != null && rightmostEnemy.transform.position.x + horizontalMoveValue 
                    > rightWallPos - screenHorizontalPadding)
                {
                    _isMovingRight = false;
                    await MoveEnemiesAsync(); 
                }
            }
            else
            {
                var leftmostEnemy = allEnemies.OrderBy(e => e.transform.position.x).FirstOrDefault();
                if (leftmostEnemy != null && leftmostEnemy.transform.position.x - horizontalMoveValue 
                    < -rightWallPos + screenHorizontalPadding)
                {
                    _isMovingRight = true;
                    await MoveEnemiesAsync(); 
                }
            }

            foreach (var enemy in allEnemies.Where(enemy => enemy != null))
            {
                enemy.transform.position += new Vector3(_isMovingRight ? horizontalMoveValue : -horizontalMoveValue, 0, 0);
                await UniTask.Delay(TimeSpan.FromSeconds(enemyHorizontalMoveInterval), cancellationToken: token);
            }
        }
        private List<GameObject> GetAllCurrentEnemies()
        {
            var allChildren = GetComponentsInChildren<Transform>();

            var childrenList = allChildren.Select(child => child.gameObject).ToList();
            return childrenList;
        }
    }
}