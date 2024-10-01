using System;
using System.Collections.Generic;
using Core;
using Enemy;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIManager:MonoBehaviour
    {
        [Header("Player Health")] 
        [SerializeField] private Image[] livesImages;

        [Header("Score")] 
        [SerializeField] private TMP_Text scoreText; 
        [SerializeField] private EnemiesMovement enemiesMovement;

        private UIPlayerHealthDisplay _healthDisplay;
        private UIScoreDisplay _scoreDisplay;
        private PlayerHealth _playerHealth;

        [Inject]
        public void Initialize(PlayerHealth playerHealth)
        {
            _playerHealth = playerHealth;
        }
        private void Start()
        {
            _healthDisplay = new UIPlayerHealthDisplay(livesImages, _playerHealth);
            _scoreDisplay = new UIScoreDisplay(_healthDisplay, scoreText, enemiesMovement.AllEnemies);
        }
    }
}