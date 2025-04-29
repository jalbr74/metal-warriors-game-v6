using System;
using Godot;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public partial class Nitro : CharacterBody2D, INitroCharacter
{
    public ISnesController Controller { get; set; } = new SnesController();
    public IConsolePrinter Console { get; set; } = new ConsolePrinter();
    public AnimatedSprite2D NitroAnimations { get; set; }
    public bool IsAnimationFinished { get; set; }
    public bool OnFloor => IsOnFloor();

    public NitroDirection Direction
    {
        set => NitroAnimations.Scale = new Vector2(value == NitroDirection.FacingLeft ? -1 : 1, NitroAnimations.Scale.Y);
        get => NitroAnimations.Scale.X >= 0 ? NitroDirection.FacingRight : NitroDirection.FacingLeft;
    }
    
    public string CurrentAnimation => NitroAnimations.Animation;

    private StateMachine _stateMachine;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
        
        _stateMachine = new StateMachine(new System.Collections.Generic.Dictionary<string, State>
        {
            {"idle", new NitroIdleState(this)},
            {"walking", new NitroWalkingState(this)},
            {"launching", new NitroLaunchingState(this)},
            {"falling", new NitroFallingState(this)},
            {"flying", new NitroFlyingState(this)},
            {"landing", new NitroLandingState(this)},
        }, "idle");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        NitroAnimations.Offset = DetermineAnimationPositionOffset();
        
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
    
    private Vector2 DetermineAnimationPositionOffset()
    {
        return NitroAnimations.Animation.ToString() switch
        {
            "flying" => new Vector2(-10, 4),
            "landing" => new Vector2(-4, 0),
            "launching" => new Vector2(-8, 5),
            _ => new Vector2(0, 0)
        };
    }
}
