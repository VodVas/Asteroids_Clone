using UnityEngine;
using Zenject;

namespace AsteroidsClone
{
    public sealed class GameControllerMono : MonoBehaviour
    {
        private GameController _gameController;

        [Inject]
        public void Construct(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _gameController.Initialize();
        }

        private void Update()
        {
            _gameController.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _gameController?.Dispose();
        }
    }
}