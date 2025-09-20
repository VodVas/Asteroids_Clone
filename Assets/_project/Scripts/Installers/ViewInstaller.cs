using Zenject;

namespace AsteroidsClone
{
    public sealed class ViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ObjectPoolManager>().AsSingle();
            Container.Bind<PlayerViewRenderer>().AsSingle();
            Container.Bind<EntityViewSynchronizer>().AsSingle();
            Container.Bind<LaserParticleBeamManager>().AsSingle();
        }
    }
}