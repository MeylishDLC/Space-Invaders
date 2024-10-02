using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sound
{
    public class MovementSoundHandler
    {
        public CancellationTokenSource MoveSoundCycleCts = new();
        public float SoundDelay { get; set; }
        
        private SoundManager _soundManager;
        private FMODEvents _fmodEvents;
        
        public MovementSoundHandler(float initialSoundDelay, SoundManager soundManager, FMODEvents fmodEvents)
        {
            SoundDelay = initialSoundDelay;
            _soundManager = soundManager;
            _fmodEvents = fmodEvents;
        }
        public async UniTask PlayMoveSoundCycle(CancellationToken token)
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(SoundDelay), cancellationToken: token);
                _soundManager.PlayOneShot(_fmodEvents.enemyWalkSound);
            }
        }
    }
}