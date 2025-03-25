using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public enum NitroDirection { Left, Right }

// Represents Nitro as a concept, and doesn't worry about the implementation details.
public interface INitro
{
    Vector2 Velocity { get; set; }
    NitroDirection Direction { get; set; }
    bool IsOnFloor();
    void PlayAnimation(string animation);
    string CurrentAnimation { get; }
}

public static class NitroDefaults
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
}

// This class operates on the INitro interface so that it can be used with any implementation of INitro (useful for doing TDD).
public class NitroHandler(ISnesController snesController, INitro nitro)
{
    [Export] public float MovementSpeed = NitroDefaults.MovementSpeed;
    [Export] public float MaxFallingVelocity = NitroDefaults.MaxFallingVelocity;
    [Export] public float MaxRisingVelocity = NitroDefaults.MaxRisingVelocity;
    [Export] public float FallingForce = NitroDefaults.FallingForce;
    [Export] public float BoostingForce = NitroDefaults.BoostingForce;

    public void PhysicsProcess(double delta)
    {
        var velocity = nitro.Velocity;
        
        if (snesController.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.Left;
            nitro.PlayAnimation("walking");
        
            velocity.X = -MovementSpeed;
        }
        else if (snesController.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.Right;
            nitro.PlayAnimation("walking");
        
            velocity.X = MovementSpeed;
        }
        else
        {
            nitro.PlayAnimation("idle");
            velocity.X = 0;
        }
        
        if (snesController.IsButtonBPressed)
        {
            if (nitro.IsOnFloor())
            {
                velocity.Y = MaxRisingVelocity;
                nitro.PlayAnimation("launching");
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
            if (nitro.IsOnFloor())
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
        
                nitro.PlayAnimation("falling");
            }
        }
        
        nitro.Velocity = velocity;
    }
}
