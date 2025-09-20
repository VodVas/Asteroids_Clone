using UnityEngine;

namespace AsteroidsClone
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Asteroids/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Screen")]
        [SerializeField] private float _screenWidth = 20f;
        [SerializeField] private float _screenHeight = 15f;

        [Header("Player")]
        [SerializeField] private float _playerAcceleration = 10f;
        [SerializeField] private float _playerMaxSpeed = 8f;
        [SerializeField] private float _playerRotationSpeed = 180f;
        [SerializeField] private float _playerDrag = 0.99f;

        [Header("Weapons")]
        [SerializeField] private float _bulletSpeed = 15f;
        [SerializeField] private float _bulletLifetime = 2f;
        [SerializeField] private int _maxLaserCharges = 3;
        [SerializeField] private float _laserRechargeTime = 5f;
        [SerializeField] private float _laserRange = 50f;
        [SerializeField] private float _laserVisualActiveTime = 1f;
        [SerializeField] private float _bulletCooldown = 0.25f;
        [SerializeField] private float _bulletPositionOffset = 0.5f;
        [SerializeField] private float _bulletInheritVelocityFactor = 0.5f;
        [SerializeField] private float _visualBulletRotationOffset = -90f;

        [Header("Asteroids")]
        [SerializeField] private float[] _asteroidSpeeds = { 2f, 3f, 4f };
        [SerializeField] private int[] _asteroidScores = { 20, 50, 100 };
        [SerializeField] private int _asteroidFragments = 2;
        [SerializeField] private float _asteroidColliderRadiusPerSize = 0.3f;
        [SerializeField] private float _asteroidFragmentOffsetDistance = 0.5f;
        [SerializeField] private float _asteroidVisualScaleFactor = 0.2f;

        [Header("UFO")]
        [SerializeField] private float _ufoSpeed = 3f;
        [SerializeField] private int _ufoScore = 200;
        [SerializeField] private float _ufoColliderRadius = 0.5f;

        [Header("Spawning")]
        [SerializeField] private float _initialSpawnDelay = 3f;
        [SerializeField] private float _minSpawnDelay = 0.5f;
        [SerializeField] private float _spawnAcceleration = 0.95f;
        [SerializeField] private int _initialAsteroidsCount = 3;
        [SerializeField] private float _ufoSpawnDelayMultiplier = 3f;
        [SerializeField] private int _defaultAsteroidSize = 3;
        [SerializeField] private float _edgeSpawnMargin = 1f;

        [Header("View")]
        [SerializeField] private int _asteroidPoolInitial = 20;
        [SerializeField] private int _bulletPoolInitial = 30;
        [SerializeField] private int _ufoPoolInitial = 5;
        [SerializeField] private float _playerViewRotationOffset = 270f;

        [Header("Collisions")]
        [SerializeField] private float _defaultColliderRadius = 0.3f;

        [Header("Input config")]
        [SerializeField] private KeyCode _thrustKey = KeyCode.W;
        [SerializeField] private KeyCode _bulletKey = KeyCode.Space;
        [SerializeField] private KeyCode _laserKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode _restartKey = KeyCode.R;
        [SerializeField] private string _rotationAxis = "Horizontal";

        public KeyCode ThrustKey => _thrustKey;
        public KeyCode BulletKey => _bulletKey;
        public KeyCode LaserKey => _laserKey;
        public KeyCode RestartKey => _restartKey;
        public string RotationAxis => _rotationAxis;
        public float ScreenWidth => _screenWidth;
        public float ScreenHeight => _screenHeight;
        public float PlayerAcceleration => _playerAcceleration;
        public float PlayerMaxSpeed => _playerMaxSpeed;
        public float PlayerRotationSpeed => _playerRotationSpeed;
        public float PlayerDrag => _playerDrag;
        public float BulletSpeed => _bulletSpeed;
        public float BulletLifetime => _bulletLifetime;
        public int MaxLaserCharges => _maxLaserCharges;
        public float LaserRechargeTime => _laserRechargeTime;
        public float LaserRange => _laserRange;
        public float LaserVisualActiveTime => _laserVisualActiveTime;
        public float BulletCooldown => _bulletCooldown;
        public float BulletPositionOffset => _bulletPositionOffset;
        public float BulletInheritVelocityFactor => _bulletInheritVelocityFactor;
        public float VisualBulletRotationOffset => _visualBulletRotationOffset;
        public float[] AsteroidSpeeds => _asteroidSpeeds;
        public int[] AsteroidScores => _asteroidScores;
        public int AsteroidFragments => _asteroidFragments;
        public float AsteroidColliderRadiusPerSize => _asteroidColliderRadiusPerSize;
        public float AsteroidFragmentOffsetDistance => _asteroidFragmentOffsetDistance;
        public float AsteroidVisualScaleFactor => _asteroidVisualScaleFactor;
        public float UfoSpeed => _ufoSpeed;
        public int UfoScore => _ufoScore;
        public float UfoColliderRadius => _ufoColliderRadius;
        public float InitialSpawnDelay => _initialSpawnDelay;
        public float MinSpawnDelay => _minSpawnDelay;
        public float SpawnAcceleration => _spawnAcceleration;
        public int InitialAsteroidsCount => _initialAsteroidsCount;
        public float UfoSpawnDelayMultiplier => _ufoSpawnDelayMultiplier;
        public int DefaultAsteroidSize => _defaultAsteroidSize;
        public float EdgeSpawnMargin => _edgeSpawnMargin;
        public int AsteroidPoolInitial => _asteroidPoolInitial;
        public int BulletPoolInitial => _bulletPoolInitial;
        public int UfoPoolInitial => _ufoPoolInitial;
        public float PlayerViewRotationOffset => _playerViewRotationOffset;
        public float DefaultColliderRadius => _defaultColliderRadius;
    }
}