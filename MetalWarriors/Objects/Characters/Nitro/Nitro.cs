using Godot;
using System;
using System.Security.Cryptography;

public static class NitroDefaults
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
}

public enum NitroDirection { Left, Right }

public partial class Nitro : CharacterBody2D
{
    public AnimatedSprite2D NitroAnimations { get; set; }
    
    [Export] public float MovementSpeed = NitroDefaults.MovementSpeed;
    [Export] public float MaxFallingVelocity = NitroDefaults.MaxFallingVelocity;
    [Export] public float MaxRisingVelocity = NitroDefaults.MaxRisingVelocity;
    [Export] public float FallingForce = NitroDefaults.FallingForce;
    [Export] public float BoostingForce = NitroDefaults.BoostingForce;

    // These are currently only used for testing, so maybe we should move them to be extension methods
    public NitroDirection Direction => NitroAnimations.Scale.X > 0 ? NitroDirection.Right : NitroDirection.Left;
    public string CurrentAnimation => NitroAnimations.Animation;
    // End of testing properties
    
    public override void _Ready()
    {
        NitroAnimations = GetNode<AnimatedSprite2D>("NitroAnimations");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        
        if (Input.IsActionPressed("D_Pad_Left"))
        {
            NitroAnimations.Scale = new Vector2(-1, NitroAnimations.Scale.Y);
            NitroAnimations.Play("walking");
            
            velocity.X = -MovementSpeed;
        }
        else if (Input.IsActionPressed("D_Pad_Right", false))
        {
            NitroAnimations.Scale = new Vector2(1, NitroAnimations.Scale.Y);
            NitroAnimations.Play("walking");
            
            velocity.X = MovementSpeed;
        }
        else
        {
            NitroAnimations.Play("idle");
            velocity.X = 0;
        }

        if (Input.IsActionPressed("Button_B", false))
        {
            if (IsOnFloor())
            {
                velocity.Y = MaxRisingVelocity;
                NitroAnimations.Play("launching");
            }
            else
            {
                velocity.Y -= BoostingForce;
                
                if (velocity.Y < MaxRisingVelocity)
                {
                    velocity.Y = MaxRisingVelocity;
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
                velocity.Y += FallingForce;

                if (velocity.Y > MaxFallingVelocity)
                {
                    velocity.Y = MaxFallingVelocity;
                }
                
                NitroAnimations.Play("falling");
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
