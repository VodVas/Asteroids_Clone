using System;
using UnityEngine;

namespace AsteroidsClone
{
    public sealed class GameController : IDisposable
    {
        private readonly GameOrchestrator _orchestrator;

        public event Action<Vector2, Vector2> OnLaserFired
        {
            add => _orchestrator.OnLaserFired += value;
            remove => _orchestrator.OnLaserFired -= value;
        }

        public GameState GameState => _orchestrator.GameState;
        public Player Player => _orchestrator.Player;
        public EntityRegistry EntityManager => _orchestrator.EntityManager;

        public GameController(GameOrchestrator orchestrator)
        {
            _orchestrator = orchestrator ?? throw new ArgumentNullException(nameof(orchestrator));
        }

        public void Initialize()
        {
            _orchestrator.Initialize();
        }

        public void Update(float deltaTime)
        {
            _orchestrator.Update(deltaTime);
        }

        public void Dispose()
        {
            _orchestrator?.Dispose();
        }
    }
}