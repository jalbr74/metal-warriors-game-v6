using Godot;
using System;

public partial class Nitro : CharacterBody2D
{
    public AnimatedSprite2D NitroAnimations { get; set; }
    
    public const float JetVelocity = -300.0f;
    public const float Gravity = 10.0f;
    public const float TerminalFallVelocity = 300.0f;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("Button_B", false))
        {
            Velocity = new Vector2(0, JetVelocity);
        }
        else
        {
            Velocity = new Vector2(0, Velocity.Y + Gravity);
            
            if (Velocity.Y > TerminalFallVelocity)
            {
                Velocity = new Vector2(0, TerminalFallVelocity);
            }
        }

        
        MoveAndSlide();
    }
}
