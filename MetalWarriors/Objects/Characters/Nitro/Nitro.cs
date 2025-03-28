using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public partial class Nitro : CharacterBody2D, INitro
{
    public AnimatedSprite2D NitroAnimations { get; set; }

    public NitroDirection Direction
    {
        set => NitroAnimations.Scale = new Vector2(value == NitroDirection.Left ? -1 : 1, NitroAnimations.Scale.Y);
        get => NitroAnimations.Scale.X >= 0 ? NitroDirection.Right : NitroDirection.Left;
    }
    
    public string CurrentAnimation => NitroAnimations.Animation;

    private NitroHandler _nitroHandler;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
        
        _nitroHandler = new NitroHandler(new SnesController(), this, new ConsolePrinter());
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _nitroHandler.PhysicsProcess(delta);
        MoveAndSlide();
    }

    public void AnimationFinished()
    {
        GD.Print("Animation finished");

        if (NitroAnimations.Animation != "launching") return;
        
        _nitroHandler.LaunchingAnimationFinished();
    }

    public void PlayAnimation(string animation)
    {
        if (NitroAnimations.Animation == animation) return;

        GD.Print($"Playing animation: {animation}");
        NitroAnimations.Play(animation);
    }
}
