using Core;
using Input;
using Player;
using Sound;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private InputListener inputListener;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private SoundManager soundManager;
        public override void InstallBindings()
        {
            BindSoundManager();
            BindInputListener();
            BindPlayerHealth();
            BindSceneController();
        }
        private void BindSoundManager()
        {
            Container.Bind<SoundManager>().FromInstance(soundManager).AsSingle();

            var child = soundManager.gameObject.transform.GetChild(0);
            Container.Bind<FMODEvents>().FromComponentOn(child.gameObject).AsSingle();
        }
        private void BindInputListener()
        {
            Container.Bind<InputListener>().FromInstance(inputListener).AsSingle();
        } 
        private void BindPlayerHealth()
        {
            Container.Bind<PlayerHealth>().FromInstance(playerHealth).AsSingle();
        }
        private void BindSceneController()
        {
            Container.Bind<SceneController>().AsSingle();
        }
    }
}