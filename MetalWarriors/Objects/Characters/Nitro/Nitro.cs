using Godot;
using System;

public partial class Nitro : CharacterBody2D
{
    public AnimatedSprite2D NitroAnimations { get; set; }
    
    public const float MovementSpeed = 300.0f;
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
        var velocity = Velocity;
        
        if (Input.IsActionPressed("D_Pad_Left"))
        {
            // NitroAnimations.FlipH = true;
            velocity.X = -MovementSpeed;
        }
        else if (Input.IsActionPressed("D_Pad_Right", false))
        {
            // NitroAnimations.FlipH = false;
            velocity.X = MovementSpeed;
        }
        else
        {
            velocity.X = 0;
        }

        if (Input.IsActionPressed("Button_B", false))
        {
            if (IsOnFloor())
            {
                velocity.Y = MaxRisingVelocity;
            }
            else
            {
                velocity.Y = Velocity.Y - BoostingForce;
                
                if (Velocity.Y < MaxRisingVelocity)
                {
                    velocity.Y = Velocity.Y - MaxRisingVelocity;
                }
            }
        }
        else
        {
            if (IsOnFloor())
            {
                velocity.Y = 0;
            }
            else
            {
                velocity.Y = Velocity.Y + FallingForce;

                if (Velocity.Y > MaxFallingVelocity)
                {
                    velocity.Y = MaxFallingVelocity;
                }
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
