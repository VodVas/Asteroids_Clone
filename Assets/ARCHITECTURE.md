# Architecture Deep Dive

Подробное описание архитектурных решений проекта Asteroids Clone.

## 🏛️ Общая архитектура

### Слоистая архитектура

```
┌─────────────────────────────────────────────────────────────┐
│                    View Layer                               │
│  ┌─────────────────┐  ┌─────────────────┐  ┌──────────────┐ │
│  │ VisualRenderer  │  │ UIView          │  │ Gizmos       │ │
│  │ ObjectPoolMgr   │  │ ThrusterToggler │  │ Debug        │ │
│  └─────────────────┘  └─────────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                  Game Services                              │
│  ┌─────────────────┐  ┌─────────────────┐  ┌──────────────┐ │
│  │ GameOrchestrator│  │ CollisionDetect │  │EntityFactory │ │
│  │ PlayerController│  │ WeaponController│  │EntityRegistry│ │
│  └─────────────────┘  └─────────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                    Core Layer                               │
│  ┌─────────────────┐  ┌─────────────────┐  ┌──────────────┐ │
│  │ Player          │  │ Asteroid        │  │ Bullet       │ │
│  │ GameState       │  │ Ufo             │  │ GameConfig   │ │
│  └─────────────────┘  └─────────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                    Interfaces                               │
│ IGameEntity | IEntityManager | IInputService | ISpawnService│
└─────────────────────────────────────────────────────────────┘
```

### Принципы проектирования

#### 1. Single Responsibility Principle (SRP)
Каждый класс имеет одну причину для изменения:
- `Player` - только логика игрока
- `CollisionDetector` - только обработка коллизий
- `EntityFactory` - только создание сущностей
- `UIView` - только отображение интерфейса

#### 2. Open/Closed Principle (OCP)
Система открыта для расширения, закрыта для модификации:
- Новые типы сущностей через реализацию `IGameEntity`
- Новые виды оружия через дополнительные контроллеры
- Новые системы ввода через реализацию `IInputService`

#### 3. Liskov Substitution Principle (LSP)
Все реализации интерфейсов взаимозаменяемы:
- Любая реализация `IGameEntity` работает в `EntityRegistry`
- Любая реализация `IInputService` работает с контроллерами

#### 4. Interface Segregation Principle (ISP)
Интерфейсы сфокусированы на конкретных задачах:
- `IGameEntity` - только базовые свойства сущности
- `ISpawnService` - только методы создания объектов
- `ICollisionDetector` - только обработка коллизий

#### 5. Dependency Inversion Principle (DIP)
Высокоуровневые модули не зависят от низкоуровневых:
- `GameOrchestrator` зависит от абстракций, не от конкретных классов
- Все зависимости инжектируются через Zenject

## 🔄 Жизненный цикл игры

### Инициализация
```
Unity Start
    ↓
Zenject InstallBindings
    ↓
ConfigValidator.Validate()
    ↓
GameController.Initialize()
    ↓
GameOrchestrator.Initialize()
    ↓
SpawnInitialAsteroids()
```

### Игровой цикл (каждый кадр)
```
Update (GameOrchestrator)
    ↓
1. HandleRestart() - проверка перезапуска
    ↓
2. PlayerController.Update() - обновление игрока
    ↓
3. WeaponController.Update() - обработка стрельбы
    ↓
4. EntityController.Update() - обновление всех сущностей
    ↓
5. EntityFactory.Update() - спавн новых объектов
    ↓
6. CollisionDetector.CheckCollisions() - обработка коллизий
    ↓
7. EntityRegistry.ProcessChanges() - применение изменений

LateUpdate (VisualRenderer)
    ↓
1. PlayerViewRenderer.UpdatePlayerView()
    ↓
2. EntityViewSynchronizer.UpdateEntityViews()
    ↓
3. EntityViewSynchronizer.CleanupDestroyedViews()
```

## 🎯 Управление сущностями

### Entity-Component-System (упрощенная версия)

#### Entities (Сущности)
Все игровые объекты реализуют `IGameEntity`:
```csharp
public interface IGameEntity
{
    int Id { get; }                    // Уникальный идентификатор
    Vector2 Position { get; }          // Позиция в мире
    Vector2 Velocity { get; }          // Скорость движения
    float Rotation { get; }            // Поворот
    bool IsActive { get; }             // Активность сущности
    EntityType Type { get; }           // Тип для полиморфизма
    void Update(float deltaTime, GameConfig config);  // Обновление логики
    void Destroy();                    // Деактивация
}
```

#### Systems (Системы)
Системы обрабатывают сущности:
- `EntityController` - обновляет все сущности
- `CollisionDetector` - проверяет взаимодействия
- `EntityViewSynchronizer` - синхронизирует визуал

#### Registry (Реестр)
`EntityRegistry` управляет коллекцией сущностей:
```csharp
// Буферизация операций для безопасности
private readonly List<IGameEntity> _entitiesToAdd = new();
private readonly List<IGameEntity> _entitiesToRemove = new();

// Применение изменений в конце кадра
public void ProcessChanges()
{
    // Сначала удаляем
    foreach (var entity in _entitiesToRemove)
        _entities.Remove(entity);
    
    // Потом добавляем
    foreach (var entity in _entitiesToAdd)
        _entities.Add(entity);
        
    // Очищаем буферы
    _entitiesToAdd.Clear();
    _entitiesToRemove.Clear();
}
```

## ⚡ Система коллизий

### Алгоритм обнаружения
```csharp
public void CheckCollisions()
{
    // 1. Проверяем коллизии игрока с враждебными объектами
    if (_player.IsAlive)
    {
        foreach (var entity in entities)
        {
            if (entity.Type == EntityType.Asteroid || entity.Type == EntityType.Ufo)
            {
                if (CheckCollision(_player.Position, entity.Position, GetCollisionRadius(entity)))
                {
                    _player.Kill();
                    _gameState.GameOver();
                    return; // Игра окончена
                }
            }
        }
    }

    // 2. Проверяем попадания пуль по целям
    var bullets = GetActiveBullets();
    foreach (var bullet in bullets)
    {
        foreach (var target in GetTargets())
        {
            if (CheckCollision(bullet.Position, target.Position, GetCollisionRadius(target)))
            {
                HandleBulletHit(bullet, target);
                break; // Пуля уничтожена
            }
        }
    }
}
```

### Оптимизации
- **Broad Phase** - грубая фильтрация по расстоянию
- **Narrow Phase** - точная проверка пересечения окружностей

## 🏭 Factory Pattern для создания сущностей

### EntityFactory
```csharp
public void SpawnAsteroid(Vector2? position = null, int size = 0)
{
    // Определяем позицию спавна
    var spawnPos = position ?? GetRandomEdgePosition();
    
    // Определяем размер
    var asteroidSize = size == 0 ? _config.DefaultAsteroidSize : size;
    
    // Вычисляем скорость по размеру
    var velocity = GetRandomVelocity(_config.AsteroidSpeeds[3 - asteroidSize]);
    
    // Создаем сущность
    var asteroid = new Asteroid(_gameState.GetNextEntityId(), spawnPos, velocity, asteroidSize);
    
    // Регистрируем в системе
    _entityManager.AddEntity(asteroid);
}
```

### Преимущества
- **Централизованное создание** - все параметры в одном месте
- **Конфигурируемость** - параметры из GameConfig
- **ID management** - автоматическая генерация уникальных ID
- **Тайминги спавна** - управляемое появление объектов

## 🎮 Система ввода

### Абстракция ввода
```csharp
public interface IInputService
{
    bool IsThrusting { get; }      // Ускорение
    float RotationInput { get; }   // Поворот (-1 до 1)
    bool FireBullet { get; }       // Стрельба пулями
    bool FireLaser { get; }        // Лазерная стрельба
    bool RestartGame { get; }      // Перезапуск
}
```

### Реализация
```csharp
public class InputService : IInputService
{
    public bool IsThrusting => Input.GetKey(_config.ThrustKey);
    public float RotationInput => -Input.GetAxis(_config.RotationAxis);
    public bool FireBullet => Input.GetKeyDown(_config.BulletKey);
    // ...
}
```

### Преимущества абстракции
- **Тестируемость** - легко создать mock для тестов
- **Платформенная независимость** - можно заменить на мобильный ввод
- **Переназначение клавиш** - через конфигурацию

## 💾 Управление состоянием

### GameState - центральное хранилище
```csharp
public class GameState
{
    // События для слабой связности
    public event Action<int> OnScoreChanged;
    public event Action OnGameOver;
    public event Action OnGameRestarted;
    
    // Инкапсулированное состояние
    private int _score;
    private bool _isGameOver;
    private int _nextEntityId;
    
    // Контролируемое изменение состояния
    public void AddScore(int points)
    {
        _score += points;
        OnScoreChanged?.Invoke(_score); // Уведомляем подписчиков
    }
}
```

### Observer Pattern
- **UI** подписывается на изменения счета
- **Visual Effects** реагируют на события игры
- **Audio System** воспроизводит звуки по событиям

## 🔧 Dependency Injection (Zenject)

### Структура инсталляторов
```
CoreInstaller (базовые сервисы)
├── GameConfig
├── GameState  
├── Player
└── IInputService

EntityInstaller (управление сущностями)
├── EntityRegistry
├── EntityFactory
└── IEntityManager

GameplayInstaller (игровая логика)
├── CollisionDetector
├── PlayerController
├── WeaponController
└── GameOrchestrator

ViewInstaller (визуальный слой)
├── ObjectPoolManager
├── PlayerViewRenderer
├── EntityViewSynchronizer
└── LaserParticleBeamManager
```

### Преимущества DI
- **Инверсия зависимостей** - высокоуровневые модули не знают о низкоуровневых
- **Тестируемость** - легкая подмена зависимостей в тестах
- **Единственная точка конфигурации** - все зависимости в одном месте
- **Автоматическое управление жизненным циклом** - Zenject создает и уничтожает объекты

## 🎨 Визуальный слой

### Синхронизация Model-View
```
Entity (Model)          GameObject (View)
     │                        │
     │ Position changed       │
     ├───────────────────────►│ transform.position = entity.Position
     │                        │
     │ Rotation changed       │
     ├───────────────────────►│ transform.rotation = Quaternion.Euler(0, 0, entity.Rotation)
     │                        │
     │ Destroyed              │
     ├───────────────────────►│ ReturnToPool(gameObject)
```

### Object Pooling
```csharp
// Получение из пула
public GameObject GetFromPool(EntityType entityType)
{
    var pool = _pools[entityType];
    if (pool.Count > 0)
        return pool.Dequeue();
    
    // Создаем новый, если пул пуст
    return Object.Instantiate(_prefabs[entityType]);
}

// Возврат в пул
public void ReturnToPool(GameObject obj, EntityType entityType)
{
    obj.SetActive(false);
    _pools[entityType].Enqueue(obj);
}
```

## 📊 Производительность и оптимизации

### Критические участки
1. **Коллизии** - O(N²) сложность при большом количестве объектов
2. **Синхронизация View** - создание/уничтожение GameObjects
3. **Спавн сущностей** - частые аллокации памяти

### Примененные оптимизации
1. **Object Pooling** - переиспользование GameObjects
2. **Буферизация операций** - отложенное добавление/удаление сущностей
3. **HashSet lookup** - O(1) проверка активности сущностей
4. **Избегание LINQ** - в горячих путях используются циклы
5. **Кэширование компонентов** - сохранение ссылок на Transform