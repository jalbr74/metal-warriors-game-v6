using Godot;
using System;

public partial class Nitro : CharacterBody2D
{
    public AnimatedSprite2D NitroAnimations { get; set; }
    
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -300.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("Button_B", false))
        {
            if (IsOnFloor()) 
            {
                Velocity = new Vector2(0, MaxRisingVelocity);
            }
            else
            {
                Velocity = new Vector2(0, Velocity.Y - BoostingForce);
                
                if (Velocity.Y < MaxRisingVelocity)
                {
                    Velocity = new Vector2(0, MaxRisingVelocity);
                }
            }
        }
        else
        {
            Velocity = new Vector2(0, Velocity.Y + FallingForce);
            
            if (Velocity.Y > MaxFallingVelocity)
            {
                Velocity = new Vector2(0, MaxFallingVelocity);
            }
        }

        
        MoveAndSlide();
    }
}
