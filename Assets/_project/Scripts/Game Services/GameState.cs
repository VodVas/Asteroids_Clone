using System;

namespace AsteroidsClone
{
    public sealed class GameState
    {
        private int _nextEntityId;

        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;
        public event Action OnGameRestarted;

        public int Score { get; private set; }
        public bool IsGameOver { get; private set; }

        public void AddScore(int points)
        {
            Score += points;
            OnScoreChanged?.Invoke(Score);
        }

        public void GameOver()
        {
            IsGameOver = true;
            OnGameOver?.Invoke();
        }

        public void Reset()
        {
            Score = 0;
            IsGameOver = false;
            _nextEntityId = 1;
            OnGameRestarted?.Invoke();
            OnScoreChanged?.Invoke(Score);
        }

        public int GetNextEntityId() => _nextEntityId++;
    }
}