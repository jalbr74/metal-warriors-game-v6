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

    private NitroBehavior _nitroBehavior;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
        
        _nitroBehavior = new NitroBehavior(new SnesController(), this);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _nitroBehavior.PhysicsProcess(delta);
        MoveAndSlide();
    }

    public void PlayAnimation(string animation)
    {
        NitroAnimations.Play(animation);
    }
}
