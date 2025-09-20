using System;

namespace AsteroidsClone
{
    public sealed class GameState
    {
        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;
        public event Action OnGameRestarted;

        private int _score;
        private bool _isGameOver;
        private int _nextEntityId;

        public int Score => _score;
        public bool IsGameOver => _isGameOver;

        public void AddScore(int points)
        {
            _score += points;
            OnScoreChanged?.Invoke(_score);
        }

        public void GameOver()
        {
            _isGameOver = true;
            OnGameOver?.Invoke();
        }

        public void Reset()
        {
            _score = 0;
            _isGameOver = false;
            _nextEntityId = 1;
            OnGameRestarted?.Invoke();
            OnScoreChanged?.Invoke(_score);
        }

        public int GetNextEntityId() => _nextEntityId++;
    }
}