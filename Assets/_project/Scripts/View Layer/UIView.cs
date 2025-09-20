using TMPro;
using UnityEngine;
using Zenject;

namespace AsteroidsClone
{
    public sealed class UIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _laserChargesText;
        [SerializeField] private TextMeshProUGUI _coordinatesText;
        [SerializeField] private TextMeshProUGUI _rotationText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _laserCooldownText;
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private GameObject _gameOverPanel;

        private GameController _gameController;

        [Inject]
        public void Construct(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            if (_gameController == null)
            {
                Debug.LogError("GameController is null! Check Zenject setup.");
                enabled = false;
                return;
            }

            _gameController.GameState.OnScoreChanged += UpdateScore;
            _gameController.GameState.OnGameOver += ShowGameOver;
            _gameController.GameState.OnGameRestarted += HideGameOver;
            _gameController.Player.OnLaserChargesChanged += UpdateLaserCharges;

            _gameOverPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_gameController != null)
            {
                _gameController.GameState.OnScoreChanged -= UpdateScore;
                _gameController.GameState.OnGameOver -= ShowGameOver;
                _gameController.GameState.OnGameRestarted -= HideGameOver;
                _gameController.Player.OnLaserChargesChanged -= UpdateLaserCharges;
            }
        }

        private void Update()
        {
            UpdatePlayerInfo();
        }

        private void UpdateScore(int score)
        {
            _scoreText.text = $"Score: {score}";
        }

        private void UpdateLaserCharges(int charges)
        {
            _laserChargesText.text = $"Laser: {charges}";
        }

        private void UpdatePlayerInfo()
        {
            if (_gameController == null) return;

            var player = _gameController.Player;
            _coordinatesText.text = $"Position: ({player.Position.x:F1}, {player.Position.y:F1})";
            _rotationText.text = $"Rotation: {player.Rotation:F0}°";
            _speedText.text = $"Speed: {player.Speed:F1}";
            _laserCooldownText.text = $"Cooldown: {player.LaserCooldown:F1}s";
        }

        private void ShowGameOver()
        {
            _gameOverPanel.SetActive(true);

            if (_gameController != null)
            {
                _finalScoreText.text = $"Final Score: {_gameController.GameState.Score}";
            }
        }

        private void HideGameOver()
        {
            _gameOverPanel.SetActive(false);
        }
    }
}