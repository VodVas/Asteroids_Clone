using Zenject;

namespace AsteroidsClone
{
    public sealed class EntityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EntityRegistry>().AsSingle();
            Container.Bind<IEntityManager>().To<EntityRegistry>().FromResolve();
            Container.Bind<EntityFactory>().AsSingle();
            Container.Bind<ISpawnService>().To<EntityFactory>().FromResolve();
        }
    }
}