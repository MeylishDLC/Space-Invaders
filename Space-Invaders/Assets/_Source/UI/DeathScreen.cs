using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeathScreen
    {
        private readonly Image _deathScreen;
        private readonly Button _retryButton;
        
        private readonly SceneController _sceneController;
        private readonly UIPlayerHealthDisplay _healthDisplay;
        
        private readonly UIScoreDisplay _scoreDisplay;
        private readonly TMP_Text _scoreText;
        public DeathScreen(Image deathScreen, Button retryButton, TMP_Text scoreText, 
            SceneController sceneController, UIPlayerHealthDisplay healthDisplay, UIScoreDisplay scoreDisplay)
        {
            _scoreDisplay = scoreDisplay;
            _scoreText = scoreText;
            _scoreText.text = "score : 0";
            
            _deathScreen = deathScreen;
            _deathScreen.gameObject.SetActive(false);
            
            _retryButton = retryButton;
            _retryButton.onClick.AddListener(Retry);
            
            _sceneController = sceneController;
            
            _healthDisplay = healthDisplay;
            _healthDisplay.OnGameOver += ShowDeathScreen;
        }
        private void ShowDeathScreen()
        {
            _sceneController.PauseGame();
            _deathScreen.gameObject.SetActive(true);
            _scoreText.text = "score : " + $"{_scoreDisplay.CurrentScore}";
        }
        private void Retry()
        {
            _retryButton.interactable = false;
            _healthDisplay.OnGameOver -= ShowDeathScreen;
            _sceneController.ReloadScene();
        }
    }
}