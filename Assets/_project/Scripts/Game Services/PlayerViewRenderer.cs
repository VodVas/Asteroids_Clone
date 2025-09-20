using UnityEngine;

namespace AsteroidsClone
{
    public sealed class PlayerViewRenderer
    {
        private GameConfig _config;
        private GameObject _playerPrefab;
        private GameObject _playerView;
        private ThrusterToggler _playerViewController;
        private float _playerRotateThreshold;

        public void Initialize(GameConfig config, GameObject playerPrefab)
        {
            _config = config ?? throw new System.ArgumentNullException(nameof(config));
            _playerPrefab = playerPrefab ?? throw new System.ArgumentNullException(nameof(playerPrefab));
            _playerRotateThreshold = config.PlayerViewRotationOffset;
        }

        public void CreatePlayerView()
        {
            if (_playerView == null)
            {
                _playerView = Object.Instantiate(_playerPrefab);
            }
            _playerView.SetActive(true);

            _playerViewController = _playerView.GetComponent<ThrusterToggler>();
        }

        public void UpdatePlayerView(Player player)
        {
            if (_playerView != null && player.IsAlive)
            {
                _playerView.transform.position = player.Position;
                _playerView.transform.rotation = Quaternion.Euler(0, 0, player.Rotation + _playerRotateThreshold);

                if (_playerViewController != null)
                {
                    _playerViewController.SetThrusterActive(player.IsThrusting);
                }
            }
        }

        public void SetPlayerActive(bool active)
        {
            if (_playerView != null)
            {
                _playerView.SetActive(active);
            }
        }
    }
}