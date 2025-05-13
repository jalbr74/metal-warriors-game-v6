using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroCharacterImplForTesting : INitroCharacter
{
    public ISnesController Controller { get; set; }
    public IConsolePrinter Console { get; set; }
    public Vector2 Velocity { get; set; }
    public CharacterDirection Direction { get; set; } = CharacterDirection.FacingRight;
    public string CurrentAnimation { get; set; } = "";
    public int CurrentAnimationFrame { get; set; } = 0;
    public Vector2 AnimationOffset { get; set; } = Vector2.Zero;
    public bool IsAnimationFinished { get; set; }
    public Vector2 GunOffset { get; set; }
    public bool OnFloor { get; set; }

    public List<string> PlayedAnimations { get; } = [];
    public bool AnimationWasPaused { get; private set; }
    
    public StateMachine StateMachine { get; set; }
    
    // This is used to make sure we set everything up correctly for consistency
    public void Initialize(
        bool onFloor,
        CharacterDirection direction,
        Vector2 velocity,
        Vector2 animationOffset,
        Vector2 gunOffset,
        string currentAnimation,
        int currentAnimationFrame,
        bool isAnimationFinished
    )
    {
        OnFloor = onFloor;
        Direction = direction;
        Velocity = velocity;
        AnimationOffset = animationOffset;
        GunOffset = gunOffset;
        CurrentAnimation = currentAnimation;
        CurrentAnimationFrame = currentAnimationFrame;
        IsAnimationFinished = isAnimationFinished;
        
        StateMachine = new StateMachine([
            new NitroIdleState(this),
            new NitroWalkingState(this),
            new NitroLaunchingState(this),
            new NitroFallingState(this),
            new NitroFlyingState(this),
            new NitroLandingState(this),
        ], typeof(NitroIdleState));
    }

    public void _PhysicsProcess(double delta)
    {
        StateMachine.PhysicsProcess(delta);
    }

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
