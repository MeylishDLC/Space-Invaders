using System;
using Player;
using TMPro;
using UnityEngine;
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

        private UIPlayerHealthDisplay _healthDisplay;

        [Inject]
        public void Initialize(PlayerHealth playerHealth)
        {
            _healthDisplay = new UIPlayerHealthDisplay(livesImages, playerHealth);
        }
        
    }
}