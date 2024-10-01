using System.Collections.Generic;
using System.Linq;
using Enemy;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIScoreDisplay
    {
        private readonly List<EnemyController> _allEnemies;
        private readonly UIPlayerHealthDisplay _healthDisplay;
        private readonly TMP_Text _scoreText;
        private int _currentScore;
        
        public UIScoreDisplay(UIPlayerHealthDisplay healthDisplay, TMP_Text scoreText, IEnumerable<GameObject> enemiesObjects)
        {
            _scoreText = scoreText;
            _scoreText.text = "0";
            _currentScore = 0;
            
            _healthDisplay = healthDisplay;
            _healthDisplay.OnGameOver += UnsubscribeOnEvents;

            _allEnemies = GetEnemyControllers(enemiesObjects);
            SubscribeOnEvents();
        }
        private void RefreshScore(EnemyController enemyDied)
        {
            
            
            _currentScore += enemyDied.ScoreAmount;
            _scoreText.text = _currentScore.ToString();
        }
        private void UnsubscribeOnEvents()
        {
            foreach (var enemy in _allEnemies)
            {
                if (enemy != null)
                {
                    enemy.OnEnemyDeath -= RefreshScore;
                }
            }
            _healthDisplay.OnGameOver -= UnsubscribeOnEvents;
        }
        private void SubscribeOnEvents()
        {
            foreach (var enemy in _allEnemies)
            {
                if (enemy != null)
                {
                    enemy.OnEnemyDeath += RefreshScore;
                }
            }
        }
        private List<EnemyController> GetEnemyControllers(IEnumerable<GameObject> enemies)
        {
            var controllers = enemies.Select(c => c.GetComponent<EnemyController>());
            return controllers.ToList();
        }
    }
}