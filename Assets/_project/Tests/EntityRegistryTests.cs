using NUnit.Framework;
using UnityEngine;
using System.Linq;

namespace AsteroidsClone.Tests
{
    public class EntityRegistryTests : BaseTest
    {
        private EntityRegistry _registry;
        private Asteroid _testAsteroid;

        [SetUp]
        public void SetUp()
        {
            _registry = new EntityRegistry();
            _testAsteroid = new Asteroid(1, Vector2.zero, Vector2.up, 2);
        }

        [Test]
        public void Constructor_InitializesEmptyRegistry()
        {
            Assert.AreEqual(0, _registry.Entities.Count);
        }

        [Test]
        public void AddEntity_BuffersEntityUntilProcessChanges()
        {
            _registry.AddEntity(_testAsteroid);
            
            Assert.AreEqual(0, _registry.Entities.Count);
            
            _registry.ProcessChanges();
            
            Assert.AreEqual(1, _registry.Entities.Count);
            Assert.Contains(_testAsteroid, _registry.Entities.ToList());
        }

        [Test]
        public void RemoveEntity_BuffersRemovalUntilProcessChanges()
        {
            _registry.AddEntity(_testAsteroid);
            _registry.ProcessChanges();
            Assert.AreEqual(1, _registry.Entities.Count);
            
            _registry.RemoveEntity(_testAsteroid);
            Assert.AreEqual(1, _registry.Entities.Count);
            
            _registry.ProcessChanges();
            Assert.AreEqual(0, _registry.Entities.Count);
        }

        [Test]
        public void ProcessChanges_HandlesMultipleOperations()
        {
            var asteroid1 = new Asteroid(1, Vector2.zero, Vector2.up, 1);
            var asteroid2 = new Asteroid(2, Vector2.zero, Vector2.up, 2);
            var bullet = new Bullet(3, Vector2.zero, Vector2.up, 0f, 1f);
            
            _registry.AddEntity(asteroid1);
            _registry.AddEntity(asteroid2);
            _registry.ProcessChanges();
            Assert.AreEqual(2, _registry.Entities.Count);
            
            _registry.RemoveEntity(asteroid1);
            _registry.AddEntity(bullet);
            _registry.ProcessChanges();
            
            Assert.AreEqual(2, _registry.Entities.Count);
            Assert.Contains(asteroid2, _registry.Entities.ToList());
            Assert.Contains(bullet, _registry.Entities.ToList());
            Assert.IsFalse(_registry.Entities.Contains(asteroid1));
        }

        [Test]
        public void Clear_RemovesAllEntitiesAndBuffers()
        {
            _registry.AddEntity(_testAsteroid);
            _registry.ProcessChanges();
            _registry.AddEntity(new Bullet(2, Vector2.zero, Vector2.up, 0f, 1f));
            
            _registry.Clear();
            
            Assert.AreEqual(0, _registry.Entities.Count);
            
            _registry.ProcessChanges();
            Assert.AreEqual(0, _registry.Entities.Count);
        }

        [Test]
        public void GetEntitiesOfType_ReturnsOnlyActiveEntitiesOfType()
        {
            var activeAsteroid = new Asteroid(1, Vector2.zero, Vector2.up, 1);
            var inactiveAsteroid = new Asteroid(2, Vector2.zero, Vector2.up, 2);
            var bullet = new Bullet(3, Vector2.zero, Vector2.up, 0f, 1f);
            
            inactiveAsteroid.Destroy();
            
            _registry.AddEntity(activeAsteroid);
            _registry.AddEntity(inactiveAsteroid);
            _registry.AddEntity(bullet);
            _registry.ProcessChanges();
            
            var asteroids = _registry.GetEntitiesOfType<Asteroid>().ToList();
            
            Assert.AreEqual(1, asteroids.Count);
            Assert.Contains(activeAsteroid, asteroids);
            Assert.IsFalse(asteroids.Contains(inactiveAsteroid));
        }

        [Test]
        public void GetEntitiesOfType_ReturnsEmptyWhenNoMatchingType()
        {
            _registry.AddEntity(_testAsteroid);
            _registry.ProcessChanges();
            
            var ufos = _registry.GetEntitiesOfType<Ufo>().ToList();
            
            Assert.AreEqual(0, ufos.Count);
        }
    }
}