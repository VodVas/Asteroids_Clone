namespace AsteroidsClone
{
    public interface IInputService
    {
        bool IsThrusting { get; }
        float RotationInput { get; }
        bool FireBullet { get; }
        bool FireLaser { get; }
        bool RestartGame { get; }
    }
}