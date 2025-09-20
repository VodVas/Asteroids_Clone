using NUnit.Framework;

namespace AsteroidsClone.Tests
{
    public class GameStateTests : BaseTest
    {
        private GameState _gameState;
        private bool _scoreChangedCalled;
        private bool _gameOverCalled;
        private bool _gameRestartedCalled;
        private int _lastScore;

        [SetUp]
        public void SetUp()
        {
            _gameState = new GameState();
            _scoreChangedCalled = false;
            _gameOverCalled = false;
            _gameRestartedCalled = false;
            _lastScore = 0;
            
            _gameState.OnScoreChanged += (score) => { _scoreChangedCalled = true; _lastScore = score; };
            _gameState.OnGameOver += () => _gameOverCalled = true;
            _gameState.OnGameRestarted += () => _gameRestartedCalled = true;
        }

        [Test]
        public void Constructor_InitializesWithZeroScoreAndAlive()
        {
            Assert.AreEqual(0, _gameState.Score);
            Assert.IsFalse(_gameState.IsGameOver);
        }

        [Test]
        public void AddScore_IncreasesScoreAndFiresEvent()
        {
            _gameState.AddScore(100);
            
            Assert.AreEqual(100, _gameState.Score);
            Assert.IsTrue(_scoreChangedCalled);
            Assert.AreEqual(100, _lastScore);
        }

        [Test]
        public void AddScore_AccumulatesScore()
        {
            _gameState.AddScore(50);
            _gameState.AddScore(30);
            
            Assert.AreEqual(80, _gameState.Score);
        }

        [Test]
        public void GameOver_SetsGameOverStateAndFiresEvent()
        {
            _gameState.GameOver();
            
            Assert.IsTrue(_gameState.IsGameOver);
            Assert.IsTrue(_gameOverCalled);
        }

        [Test]
        public void GameOver_CalledTwice_RemainsGameOver()
        {
            _gameState.GameOver();
            _gameState.GameOver();
            
            Assert.IsTrue(_gameState.IsGameOver);
        }

        [Test]
        public void Reset_RestoresInitialStateAndFiresEvents()
        {
            _gameState.AddScore(200);
            _gameState.GameOver();
            
            _gameState.Reset();
            
            Assert.AreEqual(0, _gameState.Score);
            Assert.IsFalse(_gameState.IsGameOver);
            Assert.IsTrue(_gameRestartedCalled);
            Assert.IsTrue(_scoreChangedCalled);
            Assert.AreEqual(0, _lastScore);
        }

        [Test]
        public void GetNextEntityId_ReturnsSequentialIds()
        {
            _gameState.Reset();
            
            Assert.AreEqual(1, _gameState.GetNextEntityId());
            Assert.AreEqual(2, _gameState.GetNextEntityId());
            Assert.AreEqual(3, _gameState.GetNextEntityId());
        }

        [Test]
        public void GetNextEntityId_ResetsAfterGameReset()
        {
            _gameState.GetNextEntityId();
            _gameState.GetNextEntityId();
            
            _gameState.Reset();
            
            Assert.AreEqual(1, _gameState.GetNextEntityId());
        }
    }
}