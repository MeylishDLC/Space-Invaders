using System;
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
        [SerializeField] private float moveLowerInterval;
        [SerializeField] private float moveLowerValue;
        [SerializeField] private float delayBetweenEnemyMoveLower;

        private CancellationTokenSource _cancelMovementLowerCts = new();

        private void Start()
        {
            LowerEnemiesCycle(_cancelMovementLowerCts.Token).Forget();
        }

        private async UniTask LowerEnemiesCycle(CancellationToken token)
        {
            while (!_cancelMovementLowerCts.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(moveLowerInterval), cancellationToken: token);
                await MoveEnemiesAsync();
            }
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

        private List<GameObject> GetAllCurrentEnemies()
        {
            var allChildren = GetComponentsInChildren<Transform>();

            var childrenList = allChildren.Select(child => child.gameObject).ToList();
            return childrenList;
        }
    }
}