using UnityEngine;

namespace AsteroidsClone.Tests
{
    public static class TestHelpers
    {
        public static GameConfig CreateValidConfig()
        {
            var config = ScriptableObject.CreateInstance<GameConfig>();
            
            var type = typeof(GameConfig);
            
            SetPrivateField(config, "_screenWidth", 20f);
            SetPrivateField(config, "_screenHeight", 15f);
            SetPrivateField(config, "_playerAcceleration", 10f);
            SetPrivateField(config, "_playerMaxSpeed", 8f);
            SetPrivateField(config, "_playerRotationSpeed", 180f);
            SetPrivateField(config, "_playerDrag", 0.99f);
            SetPrivateField(config, "_bulletSpeed", 15f);
            SetPrivateField(config, "_bulletLifetime", 2f);
            SetPrivateField(config, "_maxLaserCharges", 3);
            SetPrivateField(config, "_laserRechargeTime", 5f);
            SetPrivateField(config, "_laserRange", 50f);
            SetPrivateField(config, "_laserVisualActiveTime", 1f);
            SetPrivateField(config, "_bulletCooldown", 0.25f);
            SetPrivateField(config, "_bulletInheritVelocityFactor", 0.5f);
            SetPrivateField(config, "_asteroidSpeeds", new float[] { 2f, 3f, 4f });
            SetPrivateField(config, "_asteroidScores", new int[] { 20, 50, 100 });
            SetPrivateField(config, "_asteroidFragments", 2);
            SetPrivateField(config, "_asteroidColliderRadiusPerSize", 0.3f);
            SetPrivateField(config, "_asteroidFragmentOffsetDistance", 0.5f);
            SetPrivateField(config, "_asteroidVisualScaleFactor", 0.2f);
            SetPrivateField(config, "_defaultAsteroidSize", 3);
            SetPrivateField(config, "_ufoSpeed", 3f);
            SetPrivateField(config, "_ufoScore", 200);
            SetPrivateField(config, "_ufoColliderRadius", 0.5f);
            SetPrivateField(config, "_initialSpawnDelay", 3f);
            SetPrivateField(config, "_minSpawnDelay", 0.5f);
            SetPrivateField(config, "_spawnAcceleration", 0.95f);
            SetPrivateField(config, "_initialAsteroidsCount", 3);
            SetPrivateField(config, "_ufoSpawnDelayMultiplier", 3f);
            SetPrivateField(config, "_edgeSpawnMargin", 1f);
            SetPrivateField(config, "_asteroidPoolInitial", 20);
            SetPrivateField(config, "_bulletPoolInitial", 30);
            SetPrivateField(config, "_ufoPoolInitial", 5);
            SetPrivateField(config, "_defaultColliderRadius", 0.3f);
            SetPrivateField(config, "_thrustKey", KeyCode.W);
            SetPrivateField(config, "_bulletKey", KeyCode.Space);
            SetPrivateField(config, "_laserKey", KeyCode.LeftShift);
            SetPrivateField(config, "_restartKey", KeyCode.R);
            SetPrivateField(config, "_rotationAxis", "Horizontal");
            
            return config;
        }
        
        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
    }
}