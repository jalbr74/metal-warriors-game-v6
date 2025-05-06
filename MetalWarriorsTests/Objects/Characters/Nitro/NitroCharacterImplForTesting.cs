using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Utils;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroCharacterImplForTesting : INitroCharacter
{
    public ISnesController Controller { get; set; }
    public IConsolePrinter Console { get; set; }
    public Vector2 Velocity { get; set; }
    public NitroDirection Direction { get; set; } = NitroDirection.FacingRight;
    public string CurrentAnimation { get; set; } = "";
    public int CurrentAnimationFrame { get; set; } = 0;
    public Vector2 CurrentAnimationOffset { get; set; } = Vector2.Zero;
    public bool IsAnimationFinished { get; set; }
    public Vector2 GunPosition { get; set; }
    public bool OnFloor { get; set; }

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
