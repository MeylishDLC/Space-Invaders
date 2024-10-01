using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerHealthDisplay
    {
        public event Action OnGameOver;
        
        private Image[] _livesImages;
        private int _currentLivesAmount;
        private PlayerHealth _playerHealth;
        
        public UIPlayerHealthDisplay(Image[] livesImages, PlayerHealth playerHealth)
        {
            _livesImages = livesImages;
            _currentLivesAmount = _livesImages.Length;
            
            if (livesImages.Length > playerHealth.GetPlayerHealth()
                || livesImages.Length < playerHealth.GetPlayerHealth())
            {
                throw new Exception("UI lives images wasn't equal max player's health");
            }

            _playerHealth = playerHealth;
            _playerHealth.OnPlayerHealthChanged += RefreshLives;
        }
        private void RefreshLives(int newHealthAmount)
        {
            if (newHealthAmount <= 0)
            {
                OnGameOver?.Invoke();
                _playerHealth.OnPlayerHealthChanged -= RefreshLives;
            }
            
            _livesImages[newHealthAmount-1].gameObject.SetActive(false);
        }
    }
}