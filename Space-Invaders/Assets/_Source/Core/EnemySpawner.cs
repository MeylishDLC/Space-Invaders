using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private int numberOfEnemiesInRow = 10;
        [SerializeField] private float spacing = 1f;
        [SerializeField] private float rowSpacing = 1f;
        [SerializeField] private float initialRowYPosition;
        [SerializeField] private Transform spawnParent;
        
        private void Start()
        {
            SpawnAllTypesOfEnemies();
        }

        private void SpawnAllTypesOfEnemies()
        {
            var currentY = initialRowYPosition;

            foreach (var enemyPrefab in enemyPrefabs)
            {
                SpawnEnemiesInRow(enemyPrefab, currentY);
                currentY -= rowSpacing;
            }
        }
        private void SpawnEnemiesInRow(GameObject enemyPrefab, float yOffset)
        {
            var totalWidth = (numberOfEnemiesInRow - 1) * spacing;
            var startPosition = new Vector3(-totalWidth / 2, yOffset, 0);

            for (var i = 0; i < numberOfEnemiesInRow; i++)
            {
                var spawnPosition = transform.position + startPosition + new Vector3(i * spacing, 0, 0);
                var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemy.transform.parent = spawnParent;
            }
        }
    }
}