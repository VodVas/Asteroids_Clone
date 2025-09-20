using System;

namespace AsteroidsClone
{
    public sealed class EntityController : IDisposable
    {
        private readonly EntityRegistry _entityManager;
        private readonly GameConfig _config;
        private readonly Player _player;

        public EntityRegistry EntityManager => _entityManager;

        public EntityController(EntityRegistry entityManager, GameConfig config, Player player)
        {
            _entityManager = entityManager ?? throw new ArgumentNullException(nameof(entityManager));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public void Initialize()
        {
            _entityManager.Clear();
        }

        public void Update(float deltaTime)
        {
            UpdateEntities(deltaTime);
            _entityManager.ProcessChanges();
        }

        private void UpdateEntities(float deltaTime)
        {
            foreach (var entity in _entityManager.Entities)
            {
                entity.Update(deltaTime, _config);
                UpdateUfoTarget(entity);
            }
        }

        private void UpdateUfoTarget(IGameEntity entity)
        {
            if (entity is Ufo ufo && _player.IsAlive)
            {
                ufo.UpdateTarget(_player.Position, _config.UfoSpeed);
            }
        }

        public void Dispose()
        {
        }
    }
}