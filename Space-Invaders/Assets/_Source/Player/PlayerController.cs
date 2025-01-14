using System;
using Combat;
using Input;
using Sound;
using UnityEngine;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private PlayerBullet bulletPrefab;
        [SerializeField] private float shootingInterval;
        
        private InputListener _inputListener;
        private Rigidbody2D _rb;

        private PlayerMovement _playerMovement;
        private Attack _playerAttack;
        private SoundManager _soundManager;
        private FMODEvents _fmodEvents;

        [Inject]
        public void Initialize(InputListener inputListener, SoundManager soundManager, FMODEvents fmodEvents)
        {
            _inputListener = inputListener;
            _rb = GetComponent<Rigidbody2D>();

            _playerMovement = new PlayerMovement(moveSpeed, _inputListener, _rb);
            _playerAttack = new Attack(shootingInterval, bulletPrefab.gameObject, gameObject.transform);

            _soundManager = soundManager;
            _fmodEvents = fmodEvents;
        }
        private void FixedUpdate()
        {
            _playerMovement.HandleMovement();
        }
        private void Update()
        {
            if (_inputListener.IsFirePressed())
            {
                if (_playerAttack.CanShoot)
                {
                    _playerAttack.Shoot();
                    _soundManager.PlayOneShot(_fmodEvents.shootSound);
                }
            }
        }
    }
}
