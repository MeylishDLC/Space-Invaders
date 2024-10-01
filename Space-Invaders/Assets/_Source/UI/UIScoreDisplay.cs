using Enemy;
using TMPro;

namespace UI
{
    public class UIScoreDisplay
    {
        private EnemyController[] _allEnemies;
        private UIPlayerHealthDisplay _healthDisplay;
        private TMP_Text _scoreText;
        private int _currentScore;
        
        public UIScoreDisplay(UIPlayerHealthDisplay healthDisplay, TMP_Text scoreText, EnemyController[] allEnemies)
        {
            _scoreText = scoreText;
            _scoreText.text = "0";
            _currentScore = 0;
            
            SubscribeOnEvents();
            healthDisplay.OnGameOver += UnsubscribeOnEvents;
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
    }
}