using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using Sound;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class EnemiesMovement: MonoBehaviour
    {
        public event Action OnAllEnemiesKilled;
        public IReadOnlyList<GameObject> AllEnemies => _allEnemies;
        
        [Header("Screen")]
        [SerializeField] private float screenHorizontalPadding;
        [SerializeField] private Camera mainCamera;
        
        [Header("Movement Horizontal")] 
        [SerializeField] private float horizontalMoveValue; 
        [SerializeField] private float horizontalMoveSpeed;
        [SerializeField] private float moveSpeedIncreasePercentage;
        [SerializeField] private float enemyHorizontalMoveInterval;
        
        [Header("Movement Lower")]
        [SerializeField] private float moveLowerValue;
        [SerializeField] private float delayBetweenEnemyMoveLower;

        private const int moveSoundDelayMultiplier = 3;
        
        private bool _isMovingRight;
        private float _currentMoveSpeed;
        private float _currentSoundDelay;
        
        private List<GameObject> _allEnemies;
        private SoundManager _soundManager;
        private FMODEvents _fmodEvents;
        private MovementSoundHandler _soundHandler;

        [Inject]
        public void Initialize(SoundManager soundManager, FMODEvents fmodEvents)
        {
            _soundManager = soundManager;
            _fmodEvents = fmodEvents;

            _currentSoundDelay = horizontalMoveSpeed * moveSoundDelayMultiplier;
            _soundHandler = new MovementSoundHandler(_currentSoundDelay, _soundManager, _fmodEvents);
        }
        private void OnValidate()
        {
            if (Camera.main != null)
            {
                mainCamera = Camera.main;
            }
        }
        private void Awake()
        {
            _allEnemies = GetAllCurrentEnemies();
            _soundHandler.PlayMoveSoundCycle(_soundHandler.MoveSoundCycleCts.Token).Forget();
        }
        private void Start()
        {
            _currentMoveSpeed = horizontalMoveSpeed;
            MoveEnemiesHorizontallyCycle(CancellationToken.None).Forget();
            SubscribeOnEvents();
        }
        private void Update()
        {
            if (AllEnemiesKilled())
            {
                OnAllEnemiesKilled?.Invoke();
            }
        }
        private void OnDestroy()
        {
            _soundHandler.MoveSoundCycleCts.Cancel();
            _soundHandler.MoveSoundCycleCts.Dispose();
        }
        private async UniTask MoveEnemiesAsync(CancellationToken token)
        {
            _allEnemies = GetAllCurrentEnemies();
            _allEnemies.Reverse();
            
            foreach (var enemy in _allEnemies.Where(enemy => enemy != null))
            {
                enemy.transform.position += new Vector3(0, -moveLowerValue, 0);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenEnemyMoveLower), cancellationToken: token);
            }
        }
        
        private async UniTask MoveEnemiesHorizontallyCycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_currentMoveSpeed), cancellationToken: token);
                await MoveEnemiesHorizontally(token);
            }
        }

        private async UniTask MoveEnemiesHorizontally(CancellationToken token)
        {
            _allEnemies = GetAllCurrentEnemies();
            var rightWallPos = mainCamera.orthographicSize * mainCamera.aspect;
            
            if (_isMovingRight)
            {
                var rightmostEnemy = _allEnemies.OrderByDescending(e => e.transform.position.x).FirstOrDefault();
                if (rightmostEnemy != null && rightmostEnemy.transform.position.x + horizontalMoveValue 
                    > rightWallPos - screenHorizontalPadding)
                {
                    _isMovingRight = false;
                    await MoveEnemiesAsync(token); 
                }
            }
            else
            {
                var leftmostEnemy = _allEnemies.OrderBy(e => e.transform.position.x).FirstOrDefault();
                if (leftmostEnemy != null && leftmostEnemy.transform.position.x - horizontalMoveValue 
                    < -rightWallPos + screenHorizontalPadding)
                {
                    _isMovingRight = true;
                    await MoveEnemiesAsync(token); 
                }
            }

            foreach (var enemy in _allEnemies.Where(enemy => enemy != null))
            {
                enemy.transform.position += new Vector3(_isMovingRight ? horizontalMoveValue : -horizontalMoveValue, 0, 0);
                
                await UniTask.Delay(TimeSpan.FromSeconds(enemyHorizontalMoveInterval), cancellationToken: token);
            }
        }
        private void SpeedUpEnemies(EnemyController deadEnemy)
        {
            deadEnemy.OnEnemyDeath -= SpeedUpEnemies;

            if (AllEnemiesKilled())
            {
                OnAllEnemiesKilled?.Invoke();
                return;
            }
            var percentageAmount = _currentMoveSpeed * (moveSpeedIncreasePercentage / 100);

            _currentMoveSpeed -= percentageAmount;
            if (_currentMoveSpeed < 0.01f)
            {
                _currentMoveSpeed = 0.01f;
            }

            percentageAmount = enemyHorizontalMoveInterval * (moveSpeedIncreasePercentage / 100);
            enemyHorizontalMoveInterval -= percentageAmount;
            if (enemyHorizontalMoveInterval < 0.002f)
            {
                enemyHorizontalMoveInterval = 0.002f;
            }

            _soundManager.PlayOneShot(_fmodEvents.enemyDeathSound);
            _currentSoundDelay = _currentMoveSpeed * moveSoundDelayMultiplier;
            _soundHandler.SoundDelay = _currentSoundDelay;
            if (_soundHandler.SoundDelay < 0.5f)
            {
                _soundHandler.SoundDelay = 0.5f;
            }
        }
        private List<GameObject> GetAllCurrentEnemies()
        {
            var allEnemies = GetComponentsInChildren<EnemyController>();
            
            var enemiesTransforms = allEnemies.Select(enemy => enemy.GetComponent<Transform>()).ToList();

            var childrenList = enemiesTransforms.Select(child => child.gameObject).ToList();
            return childrenList;
        }
        private void SubscribeOnEvents()
        {
            var enemyObjects = GetAllCurrentEnemies();
            foreach (var enemy in enemyObjects)
            {
                var enemyController =  enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.OnEnemyDeath += SpeedUpEnemies;
                }
            }
        }

        private bool AllEnemiesKilled()
        {
            if (!_allEnemies.Any())
            {
                return true;
            }

            foreach (var enemy in _allEnemies)
            {
                if (enemy != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}