using UnityEngine;
using Zenject;

namespace AsteroidsClone
{
    public sealed class CoreInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            ConfigValidator.Validate(_gameConfig);

            Container.Bind<IInputService>().To<InputService>().AsSingle();
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
            Container.Bind<GameState>().AsSingle();
            Container.Bind<Player>().AsSingle();
        }
    }
}