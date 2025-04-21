using Godot;
using MetalWarriors.Objects.Characters.Nitro;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroCharacterImplForTesting : INitroCharacter
{
    public Vector2 Velocity { get; set; }
    public NitroDirection Direction { get; set; } = NitroDirection.Right;
    public string CurrentAnimation { get; set; } = "";
    public NitroState State { get; set; }
    public bool OnFloor { get; set; }
    public bool IsLaunchingAnimationComplete { get; set; }

    public List<string> PlayedAnimations { get; } = [];
    public bool AnimationWasPaused { get; private set; }
    
    public void PlayAnimation(string animation)
    {
        CurrentAnimation = animation;
        
        PlayedAnimations.Add(animation);
    }

    public void PauseAnimation()
    {
        AnimationWasPaused = true;
    }
}
