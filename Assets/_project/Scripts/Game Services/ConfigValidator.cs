using System;
using UnityEngine;

namespace AsteroidsClone
{
    public static class ConfigValidator
    {
        public static void Validate(GameConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "GameConfig cannot be null");

            ValidateScreen(config);
            ValidatePlayer(config);
            ValidateWeapons(config);
            ValidateAsteroids(config);
            ValidateUfo(config);
            ValidateSpawning(config);
            ValidateView(config);
            ValidateCollisions(config);
            ValidateInput(config);
        }

        private static void ValidateScreen(GameConfig config)
        {
            ValidatePositive(config.ScreenWidth, nameof(config.ScreenWidth));
            ValidatePositive(config.ScreenHeight, nameof(config.ScreenHeight));
        }

        private static void ValidatePlayer(GameConfig config)
        {
            ValidatePositive(config.PlayerAcceleration, nameof(config.PlayerAcceleration));
            ValidatePositive(config.PlayerMaxSpeed, nameof(config.PlayerMaxSpeed));
            ValidatePositive(config.PlayerRotationSpeed, nameof(config.PlayerRotationSpeed));
            ValidateRange(config.PlayerDrag, 0f, 1f, nameof(config.PlayerDrag));
        }

        private static void ValidateWeapons(GameConfig config)
        {
            ValidatePositive(config.BulletSpeed, nameof(config.BulletSpeed));
            ValidatePositive(config.BulletLifetime, nameof(config.BulletLifetime));
            ValidatePositive(config.MaxLaserCharges, nameof(config.MaxLaserCharges));
            ValidatePositive(config.LaserRechargeTime, nameof(config.LaserRechargeTime));
            ValidatePositive(config.LaserRange, nameof(config.LaserRange));
            ValidatePositive(config.LaserVisualActiveTime, nameof(config.LaserVisualActiveTime));
            ValidatePositive(config.BulletCooldown, nameof(config.BulletCooldown));
            ValidateRange(config.BulletInheritVelocityFactor, 0f, 1f, nameof(config.BulletInheritVelocityFactor));
        }

        private static void ValidateAsteroids(GameConfig config)
        {
            ValidateArrayLength(config.AsteroidSpeeds, 3, nameof(config.AsteroidSpeeds));
            ValidateArrayLength(config.AsteroidScores, 3, nameof(config.AsteroidScores));

            for (int i = 0; i < config.AsteroidSpeeds.Length; i++)
                ValidatePositive(config.AsteroidSpeeds[i], $"{nameof(config.AsteroidSpeeds)}[{i}]");

            for (int i = 0; i < config.AsteroidScores.Length; i++)
                ValidatePositive(config.AsteroidScores[i], $"{nameof(config.AsteroidScores)}[{i}]");

            ValidateNonNegative(config.AsteroidFragments, nameof(config.AsteroidFragments));
            ValidatePositive(config.AsteroidColliderRadiusPerSize, nameof(config.AsteroidColliderRadiusPerSize));
            ValidatePositive(config.AsteroidFragmentOffsetDistance, nameof(config.AsteroidFragmentOffsetDistance));
            ValidatePositive(config.AsteroidVisualScaleFactor, nameof(config.AsteroidVisualScaleFactor));
            ValidateRange(config.DefaultAsteroidSize, 1, 3, nameof(config.DefaultAsteroidSize));
        }

        private static void ValidateUfo(GameConfig config)
        {
            ValidatePositive(config.UfoSpeed, nameof(config.UfoSpeed));
            ValidatePositive(config.UfoScore, nameof(config.UfoScore));
            ValidatePositive(config.UfoColliderRadius, nameof(config.UfoColliderRadius));
        }

        private static void ValidateSpawning(GameConfig config)
        {
            ValidatePositive(config.InitialSpawnDelay, nameof(config.InitialSpawnDelay));
            ValidatePositive(config.MinSpawnDelay, nameof(config.MinSpawnDelay));
            ValidateRange(config.SpawnAcceleration, 0.01f, 0.99f, nameof(config.SpawnAcceleration));
            ValidatePositive(config.InitialAsteroidsCount, nameof(config.InitialAsteroidsCount));
            ValidatePositive(config.UfoSpawnDelayMultiplier, nameof(config.UfoSpawnDelayMultiplier));
            ValidatePositive(config.EdgeSpawnMargin, nameof(config.EdgeSpawnMargin));

            if (config.MinSpawnDelay > config.InitialSpawnDelay)
                throw new ArgumentException($"{nameof(config.MinSpawnDelay)} ({config.MinSpawnDelay}) cannot be greater than {nameof(config.InitialSpawnDelay)} ({config.InitialSpawnDelay})");
        }

        private static void ValidateView(GameConfig config)
        {
            ValidatePositive(config.AsteroidPoolInitial, nameof(config.AsteroidPoolInitial));
            ValidatePositive(config.BulletPoolInitial, nameof(config.BulletPoolInitial));
            ValidatePositive(config.UfoPoolInitial, nameof(config.UfoPoolInitial));
        }

        private static void ValidateCollisions(GameConfig config)
        {
            ValidatePositive(config.DefaultColliderRadius, nameof(config.DefaultColliderRadius));
        }

        private static void ValidateInput(GameConfig config)
        {
            if (config.ThrustKey == KeyCode.None)
                throw new ArgumentException($"{nameof(config.ThrustKey)} cannot be KeyCode.None");

            if (config.BulletKey == KeyCode.None)
                throw new ArgumentException($"{nameof(config.BulletKey)} cannot be KeyCode.None");

            if (config.LaserKey == KeyCode.None)
                throw new ArgumentException($"{nameof(config.LaserKey)} cannot be KeyCode.None");

            if (config.RestartKey == KeyCode.None)
                throw new ArgumentException($"{nameof(config.RestartKey)} cannot be KeyCode.None");

            if (string.IsNullOrEmpty(config.RotationAxis))
                throw new ArgumentException($"{nameof(config.RotationAxis)} cannot be null or empty");
        }

        private static void ValidatePositive(float value, string paramName)
        {
            if (value <= 0f)
                throw new ArgumentException($"{paramName} must be positive, got {value}");
        }

        private static void ValidatePositive(int value, string paramName)
        {
            if (value <= 0)
                throw new ArgumentException($"{paramName} must be positive, got {value}");
        }

        private static void ValidateNonNegative(int value, string paramName)
        {
            if (value < 0)
                throw new ArgumentException($"{paramName} must be non-negative, got {value}");
        }

        private static void ValidateRange(float value, float min, float max, string paramName)
        {
            if (value < min || value > max)
                throw new ArgumentException($"{paramName} must be between {min} and {max}, got {value}");
        }

        private static void ValidateRange(int value, int min, int max, string paramName)
        {
            if (value < min || value > max)
                throw new ArgumentException($"{paramName} must be between {min} and {max}, got {value}");
        }

        private static void ValidateArrayLength<T>(T[] array, int expectedLength, string paramName)
        {
            if (array == null)
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null");

            if (array.Length != expectedLength)
                throw new ArgumentException($"{paramName} must have exactly {expectedLength} elements, got {array.Length}");
        }
    }
}