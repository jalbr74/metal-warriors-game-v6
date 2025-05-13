using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Pilot;
using MetalWarriors.Utils;

namespace MetalWarriorsTests.Objects.Characters.Pilot;

public class PilotCharacterImplForTesting : IPilotCharacter
{
    public ISnesController Controller { get; set; }
    public IConsolePrinter Console { get; set; }
    public Vector2 Velocity { get; set; }
    public CharacterDirection Direction { get; set; } = CharacterDirection.FacingRight;
    public string CurrentAnimation { get; set; } = "";
    public int CurrentAnimationFrame { get; set; } = 0;
    public bool IsAnimationFinished { get; set; }
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
    
    // This is used to make sure we set everything up correctly for consistency
    public void SetInitialState(
        bool onFloor,
        CharacterDirection direction,
        Vector2 velocity,
        string currentAnimation,
        int currentAnimationFrame,
        bool isAnimationFinished
    )
    {
        OnFloor = onFloor;
        Direction = direction;
        Velocity = velocity;
        CurrentAnimation = currentAnimation;
        CurrentAnimationFrame = currentAnimationFrame;
        IsAnimationFinished = isAnimationFinished;
    }
}
