using UnityEngine;

namespace Core
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int numberOfEnemies = 10;
        [SerializeField] private float spacing = 1.5f;
        [SerializeField] private Transform spawnParent;

        private void Start()
        {
            SpawnEnemiesInRow();
        }
        private void SpawnEnemiesInRow()
        {
            var totalWidth = (numberOfEnemies - 1) * spacing;

            var startPosition = new Vector3(-totalWidth / 2, 0, 0);

            for (var i = 0; i < numberOfEnemies; i++)
            {
                var spawnPosition = transform.position + startPosition + new Vector3(i * spacing, 0, 0);
                var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemy.transform.parent = spawnParent;
            }
        }
    }
}