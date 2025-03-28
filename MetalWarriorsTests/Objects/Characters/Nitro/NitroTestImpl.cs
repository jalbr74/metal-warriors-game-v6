using Godot;
using MetalWarriors.Objects.Characters.Nitro;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroTestImpl : INitro
{
    public Vector2 Velocity { get; set; }
    public NitroDirection Direction { get; set; } = NitroDirection.Right;
    public string CurrentAnimation { get; private set; } = "";
    public List<string> PlayedAnimations { get; } = [];
    
    private bool _isOnFloor;
    
    public bool IsOnFloor()
    {
        return _isOnFloor;
    }
    
    public void SetIsOnFloor(bool isOnFloor)
    {
        _isOnFloor = isOnFloor;
    }

    public void PlayAnimation(string animation)
    {
        CurrentAnimation = animation;
        
        PlayedAnimations.Add(animation);
    }
}
