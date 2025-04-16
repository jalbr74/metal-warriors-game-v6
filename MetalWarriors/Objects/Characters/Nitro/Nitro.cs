using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public partial class Nitro : CharacterBody2D, INitroCharacter
{
    public AnimatedSprite2D NitroAnimations { get; set; }
    public NitroState State { get; set; }
    public bool OnFloor => IsOnFloor();

    public NitroDirection Direction
    {
        set => NitroAnimations.Scale = new Vector2(value == NitroDirection.Left ? -1 : 1, NitroAnimations.Scale.Y);
        get => NitroAnimations.Scale.X >= 0 ? NitroDirection.Right : NitroDirection.Left;
    }
    
    public string CurrentAnimation => NitroAnimations.Animation;

    private NitroCharacterHandler _nitroCharacterHandler;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
        
        _nitroCharacterHandler = new NitroCharacterHandler(new SnesController(), this, new ConsolePrinter());
    }
    
    public override void _PhysicsProcess(double delta)
    {
        NitroAnimations.Offset = DetermineAnimationPositionOffset();
        
        _nitroCharacterHandler.PhysicsProcess(delta);
        MoveAndSlide();
    }
    
    public void AnimationFinished()
    {
        GD.Print("Animation finished");

        if (NitroAnimations.Animation != "launching") return;
        
        _nitroCharacterHandler.LaunchingAnimationFinished();
    }

    public void PlayAnimation(string animation)
    {
        if (NitroAnimations.Animation == animation) return;

        GD.Print($"Playing animation: {animation}");
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
