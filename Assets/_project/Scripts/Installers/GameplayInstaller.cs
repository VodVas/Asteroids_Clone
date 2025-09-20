using Zenject;

namespace AsteroidsClone
{
    public sealed class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICollisionDetector>().To<CollisionDetector>().FromResolve();
            Container.Bind<CollisionDetector>().AsSingle();
            Container.Bind<PlayerController>().AsSingle();
            Container.Bind<WeaponController>().AsSingle();
            Container.Bind<EntityController>().AsSingle();
            Container.Bind<GameOrchestrator>().AsSingle();
            Container.Bind<GameController>().AsSingle();
        }
    }
}