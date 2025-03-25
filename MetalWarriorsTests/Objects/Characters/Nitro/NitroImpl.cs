using Godot;
using MetalWarriors.Objects.Characters.Nitro;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroImpl : INitro
{
    private bool _isOnFloor;
    private Vector2 _gravity;

    public Vector2 Velocity { get; set; }
    public NitroDirection Direction { get; set; } = NitroDirection.Right;
    public string CurrentAnimation { get; private set; } = "";
    
    public bool IsOnFloor()
    {
        return _isOnFloor;
    }
    
    public void SetIsOnFloor(bool isOnFloor)
    {
        _isOnFloor = isOnFloor;
    }

    public Vector2 GetGravity()
    {
        return _gravity;
    }

    public void SetGravity(Vector2 gravity)
    {
        _gravity = gravity;
    }

    public void PlayAnimation(string animation)
    {
        CurrentAnimation = animation;
    }
}
