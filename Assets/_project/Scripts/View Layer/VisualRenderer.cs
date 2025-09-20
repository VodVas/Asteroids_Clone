using UnityEngine;
using Zenject;

namespace AsteroidsClone
{
    public sealed class VisualRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _asteroidPrefab;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _ufoPrefab;
        [SerializeField] private ParticleSystem _laserParticlePrefab;

        private GameController _gameController;
        private GameConfig _config;
        private ObjectPoolManager _poolManager;
        private PlayerViewRenderer _playerViewManager;
        private EntityViewSynchronizer _entityViewManager;
        private LaserParticleBeamManager _laserEffectService;

        [Inject]
        public void Construct(GameController gameController, GameConfig config,
            ObjectPoolManager poolManager, PlayerViewRenderer playerViewManager,
            EntityViewSynchronizer entityViewManager, LaserParticleBeamManager laserEffectService)
        {
            _gameController = gameController;
            _config = config;
            _poolManager = poolManager;
            _playerViewManager = playerViewManager;
            _entityViewManager = entityViewManager;
            _laserEffectService = laserEffectService;
        }

        private void Start()
        {
            if (_gameController == null)
            {
                Debug.LogError("GameController is null! Check Zenject setup.");
                enabled = false;
                return;
            }

            InitializeServices();
            CreateViews();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
            _poolManager?.ClearPools();
        }

        private void LateUpdate()
        {
            if (_gameController == null) return;

            _playerViewManager.UpdatePlayerView(_gameController.Player);
            _entityViewManager.UpdateEntityViews(_gameController.EntityManager.Entities);
            _entityViewManager.CleanupDestroyedViews();
            _laserEffectService?.Update(Time.deltaTime);
        }

        private void InitializeServices()
        {
            _poolManager.Initialize(_config, _asteroidPrefab, _bulletPrefab, _ufoPrefab);
            _poolManager.InitializePools();
            _playerViewManager.Initialize(_config, _playerPrefab);
            _entityViewManager.Initialize(_poolManager, _config);
        }

        private void CreateViews()
        {
            _playerViewManager.CreatePlayerView();

            var laserParticle = Instantiate(_laserParticlePrefab);
            _laserEffectService.Initialize(laserParticle, _config);
        }

        private void SubscribeToEvents()
        {
            _gameController.Player.OnDestroyed += OnPlayerDestroyed;
            _gameController.GameState.OnGameRestarted += OnGameRestarted;
            _gameController.OnLaserFired += _laserEffectService.FireLaser;
        }

        private void UnsubscribeFromEvents()
        {
            if (_gameController != null)
            {
                _gameController.Player.OnDestroyed -= OnPlayerDestroyed;
                _gameController.GameState.OnGameRestarted -= OnGameRestarted;
                _gameController.OnLaserFired -= _laserEffectService.FireLaser;
            }
        }

        private void OnPlayerDestroyed(Player player)
        {
            _playerViewManager.SetPlayerActive(false);
        }

        private void OnGameRestarted()
        {
            _playerViewManager.SetPlayerActive(true);
            _entityViewManager.ClearAllViews();
        }
    }
}