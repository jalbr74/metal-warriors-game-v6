using Godot;
using MetalWarriors.Objects.Characters.Nitro;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroCharacterImplForTesting : INitroCharacter
{
    public Vector2 Velocity { get; set; }
    public NitroDirection Direction { get; set; } = NitroDirection.Right;
    public string CurrentAnimation { get; private set; } = "";
    public NitroState State { get; set; }
    public bool OnFloor { get; set; }

    public List<string> PlayedAnimations { get; } = [];
    
    public void PlayAnimation(string animation)
    {
        CurrentAnimation = animation;
        
        PlayedAnimations.Add(animation);
    }
}
