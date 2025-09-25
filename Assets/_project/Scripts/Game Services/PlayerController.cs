using System;

namespace AsteroidsClone
{
    public sealed class PlayerController : IDisposable
    {
        private readonly Player _player;
        private readonly GameConfig _config;
        private readonly IInputService _inputService;

        public Player Player => _player; //TODO под вопросом
        public event Action<Player> OnPlayerDestroyed;

        public PlayerController(Player player, GameConfig config, IInputService inputService)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));

            _player.OnDestroyed += OnPlayerDestroyed;
        }

        public void Initialize()
        {
            _player.Reset(_config);
        }

        public void Update(float deltaTime)
        {
            if (!_player.IsAlive) return;

            _player.Rotate(_inputService.RotationInput, deltaTime, _config);
            _player.Thrust(_inputService.IsThrusting, deltaTime, _config);
            _player.UpdatePosition(deltaTime, _config);
            _player.UpdateLaser(deltaTime, _config);
        }

        public void Dispose()
        {
            if (_player != null)
            {
                _player.OnDestroyed -= OnPlayerDestroyed;
            }
        }
    }
}