using Core;
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
        
        public DeathScreen(Image deathScreen, Button retryButton, 
            SceneController sceneController, UIPlayerHealthDisplay healthDisplay)
        {
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
        }
        private void Retry()
        {
            _retryButton.interactable = false;
            _healthDisplay.OnGameOver -= ShowDeathScreen;
            _sceneController.ReloadScene();
        }
    }
}