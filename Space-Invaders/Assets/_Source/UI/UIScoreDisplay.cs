using System.Collections.Generic;
using System.Linq;
using Enemy;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIScoreDisplay
    {
        private List<EnemyController> _allEnemies = new();
        private UIPlayerHealthDisplay _healthDisplay;
        private TMP_Text _scoreText;
        private int _currentScore;
        
        public UIScoreDisplay(UIPlayerHealthDisplay healthDisplay, TMP_Text scoreText, Transform enemiesTransform)
        {
            _scoreText = scoreText;
            _scoreText.text = "0";
            _currentScore = 0;
            
            _healthDisplay = healthDisplay;
            SubscribeOnEvents();
            _healthDisplay.OnGameOver += UnsubscribeOnEvents;

            _allEnemies = GetEnemyControllers(enemiesTransform);
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
        private List<EnemyController> GetEnemyControllers(Transform enemyContainer)
        {
            var controllers = enemyContainer.GetComponentsInChildren<EnemyController>();
            return controllers.ToList();
        }
    }
}