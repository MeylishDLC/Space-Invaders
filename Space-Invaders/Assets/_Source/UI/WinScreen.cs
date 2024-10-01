using Core;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class WinScreen
    {
        private readonly Image _winScreen;
        private readonly Button _retryButton;
        private readonly TMP_Text _scoreText;
        private readonly UIScoreDisplay _scoreDisplay;
        private readonly EnemiesMovement _enemiesMovement;
        private readonly SceneController _sceneController;
        
        public WinScreen(Image winScreen, Button retryButton, TMP_Text scoreText,
            EnemiesMovement enemiesMovement, UIScoreDisplay scoreDisplay, SceneController sceneController)
        {
            _enemiesMovement = enemiesMovement;
            _enemiesMovement.OnAllEnemiesKilled += ShowWinScreen;
            
            _scoreDisplay = scoreDisplay;
            _scoreText = scoreText;
            _scoreText.text = "score : 0";
            
            _retryButton = retryButton;
            _retryButton.onClick.AddListener(Retry);
            
            _winScreen = winScreen;
            _winScreen.gameObject.SetActive(false);

            _sceneController = sceneController;
        }
        private void ShowWinScreen()
        {
            _sceneController.PauseGame();
            _winScreen.gameObject.SetActive(true);
            _scoreText.text = "score : " + $"{_scoreDisplay.CurrentScore}";
        }
        private void Retry()
        {
            _retryButton.interactable = false;
            _enemiesMovement.OnAllEnemiesKilled -= ShowWinScreen;
            _sceneController.ReloadScene();
        }
    }
}