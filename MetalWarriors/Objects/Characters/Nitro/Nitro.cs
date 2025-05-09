using System;
using Godot;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public partial class Nitro : CharacterBody2D, INitroCharacter
{
    public ISnesController Controller { get; set; } = new NullSnesController();
    public IConsolePrinter Console { get; set; } = new ConsolePrinter();
    public AnimatedSprite2D NitroAnimations { get; set; }
    public AnimatedSprite2D GunAnimations { get; set; }
    public bool IsAnimationFinished { get; set; }
    public bool OnFloor => IsOnFloor();

    public NitroDirection Direction
    {
        set => NitroAnimations.Scale = new Vector2(value == NitroDirection.FacingLeft ? -1 : 1, NitroAnimations.Scale.Y);
        get => NitroAnimations.Scale.X >= 0 ? NitroDirection.FacingRight : NitroDirection.FacingLeft;
    }

    public Vector2 GunOffset
    {
        get => GunAnimations.Position;
        set => GunAnimations.Position = value;
    } 
    
    public string CurrentAnimation => NitroAnimations.Animation;
    public int CurrentAnimationFrame => NitroAnimations.Frame;

    public Vector2 AnimationOffset
    {
        get => NitroAnimations.Offset;
        set => NitroAnimations.Offset = value;
    }

    private StateMachine _stateMachine;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
        GunAnimations = GetNode<AnimatedSprite2D>("NitroAnimations/GunAnimations");
        
        _stateMachine = new StateMachine([
            new NitroIdleState(this),
            new NitroWalkingState(this),
            new NitroLaunchingState(this),
            new NitroFallingState(this),
            new NitroFlyingState(this),
            new NitroLandingState(this),
        ], typeof(NitroIdleState));
        
        GD.Print("Nitro is ready");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        NitroAnimations.Offset = AnimationOffset;
        
        _stateMachine.PhysicsProcess(delta);
        MoveAndSlide();
    }
    
    public void AnimationFinished()
    {
        Console.Print("Animation finished");
        
        IsAnimationFinished = true;
    }

    public void PlayAnimation(string animation)
    {
        if (NitroAnimations.Animation == animation) return;
        
        IsAnimationFinished = false;

        NitroAnimations.Play(animation);
    }
    
    public void PauseAnimation()
    {
        NitroAnimations.Pause();
    }
}
