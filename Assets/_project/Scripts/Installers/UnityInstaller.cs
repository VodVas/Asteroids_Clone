using Zenject;

namespace AsteroidsClone
{
    public sealed class UnityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameControllerMono>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<CollisionGizmosRenderer>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}