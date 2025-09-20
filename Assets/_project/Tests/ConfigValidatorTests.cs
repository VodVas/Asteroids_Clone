using NUnit.Framework;
using System;
using UnityEngine;
using AsteroidsClone;

namespace AsteroidsClone.Tests
{
    public class ConfigValidatorTests : BaseTest
    {
        private GameConfig _validConfig;

        [SetUp]
        public void SetUp()
        {
            _validConfig = TestHelpers.CreateValidConfig();
        }

        [TearDown]
        public void TearDown()
        {
            if (_validConfig != null)
                UnityEngine.Object.DestroyImmediate(_validConfig);
        }

        [Test]
        public void Validate_ValidConfig_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_NullConfig_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ConfigValidator.Validate(null));
            Assert.AreEqual("config", exception.ParamName);
        }

        [TestCase("_screenWidth", -1f)]
        [TestCase("_playerAcceleration", 0f)]
        [TestCase("_bulletSpeed", -5f)]
        public void Validate_NegativeValues_ThrowsArgumentException(string fieldName, float invalidValue)
        {
            SetPrivateField(_validConfig, fieldName, invalidValue);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_PlayerDragOutOfRange_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_playerDrag", 1.5f);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_SpawnAccelerationOutOfRange_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_spawnAcceleration", 1.5f);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_DefaultAsteroidSizeOutOfRange_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_defaultAsteroidSize", 5);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_WrongAsteroidSpeedsArrayLength_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_asteroidSpeeds", new float[] { 1f, 2f });
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_WrongAsteroidScoresArrayLength_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_asteroidScores", new int[] { 10, 20, 30, 40 });
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_MinSpawnDelayGreaterThanInitial_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_initialSpawnDelay", 1f);
            SetPrivateField(_validConfig, "_minSpawnDelay", 2f);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_NegativeAsteroidFragments_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_asteroidFragments", -1);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_ZeroAsteroidFragments_DoesNotThrow()
        {
            SetPrivateField(_validConfig, "_asteroidFragments", 0);
            
            Assert.DoesNotThrow(() => ConfigValidator.Validate(_validConfig));
        }

        [TestCase("_thrustKey")]
        [TestCase("_bulletKey")]
        [TestCase("_laserKey")]
        [TestCase("_restartKey")]
        public void Validate_KeyCodeNone_ThrowsArgumentException(string fieldName)
        {
            SetPrivateField(_validConfig, fieldName, KeyCode.None);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_EmptyRotationAxis_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_rotationAxis", "");
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        [Test]
        public void Validate_NullRotationAxis_ThrowsArgumentException()
        {
            SetPrivateField(_validConfig, "_rotationAxis", null);
            
            Assert.Throws<ArgumentException>(() => ConfigValidator.Validate(_validConfig));
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
    }
}