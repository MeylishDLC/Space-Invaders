using System;
using System.Collections.Generic;
using Core;
using Enemy;
using Input;
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

        [Header("Death Screen")] 
        [SerializeField] private Image deathScreenImage;
        [SerializeField] private Button retryButton;
        
        private SceneController _sceneController;
        private InputListener _inputListener;
        
        private UIPlayerHealthDisplay _healthDisplay;
        private UIScoreDisplay _scoreDisplay;
        private DeathScreen _deathScreen;

        [Inject]
        public void Initialize(PlayerHealth playerHealth, SceneController sceneController, InputListener inputListener)
        {
            _healthDisplay = new UIPlayerHealthDisplay(livesImages, playerHealth);
            _sceneController = sceneController;
            _inputListener = inputListener;
        }
        private void Start()
        {
            _scoreDisplay = new UIScoreDisplay(_healthDisplay, scoreText, enemiesMovement.AllEnemies);
            _deathScreen = new DeathScreen(deathScreenImage, retryButton, _sceneController, _healthDisplay);
        }
        private void Update()
        {
            if (_inputListener.IsRestartPressed())
            {
                _sceneController.ReloadScene();
            }
        }
    }
}