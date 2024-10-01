using Core;
using Input;
using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private InputListener inputListener;
        [SerializeField] private PlayerHealth playerHealth;
        public override void InstallBindings()
        {
            BindInputListener();
            BindPlayerHealth();
            BindSceneController();
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