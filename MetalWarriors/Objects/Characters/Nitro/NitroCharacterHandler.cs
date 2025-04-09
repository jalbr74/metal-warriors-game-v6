using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public enum NitroDirection { Left, Right }

// Represents Nitro as a concept, and doesn't worry about the implementation details.
public interface INitroCharacter
{
    Vector2 Velocity { get; set; }
    NitroDirection Direction { get; set; }
    string CurrentAnimation { get; }

    bool IsOnFloor();
    void PlayAnimation(string animation);
}

public static class NitroDefaults
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
}

public enum NitroState
{
    Idle,
    Walking,
    Launching,
    Falling,
    Flying
}

// This class operates on the INitro interface so that it can be used with any implementation of INitro (useful for doing TDD).
public class NitroCharacterHandler(ISnesController snesController, INitroCharacter nitroCharacter, IConsolePrinter consolePrinter)
{
    // Not sure if Export makes sense any more, since this isn't a Godot Node.
    [Export] public float MovementSpeed = NitroDefaults.MovementSpeed;
    [Export] public float MaxFallingVelocity = NitroDefaults.MaxFallingVelocity;
    [Export] public float MaxRisingVelocity = NitroDefaults.MaxRisingVelocity;
    [Export] public float FallingForce = NitroDefaults.FallingForce;
    [Export] public float BoostingForce = NitroDefaults.BoostingForce;
    
    public NitroState NitroState { get; set; } = NitroState.Idle;

    public void PhysicsProcess(double delta)
    {
        var velocity = nitroCharacter.Velocity;
        var animation = nitroCharacter.CurrentAnimation;
        
        if (snesController.IsDPadLeftPressed)
        {
            nitroCharacter.Direction = NitroDirection.Left;
            animation = "walking";
        
            velocity.X = -MovementSpeed;
        }
        else if (snesController.IsDPadRightPressed)
        {
            nitroCharacter.Direction = NitroDirection.Right;
            animation = "walking";
        
            velocity.X = MovementSpeed;
        }
        else
        {
            animation = "idle";
            velocity.X = 0;
        }
        
        if (snesController.IsButtonBPressed)
        {
            if (nitroCharacter.IsOnFloor())
            {
                velocity.Y = MaxRisingVelocity;
                animation = "launching";
                NitroState = NitroState.Launching;
            }
            else
            {
                velocity.Y -= BoostingForce;
        
                if (velocity.Y < MaxRisingVelocity)
                {
                    velocity.Y = MaxRisingVelocity;
                }

                animation = NitroState == NitroState.Flying ? "flying" : "launching";
            }
        }
        else
        {
            if (nitroCharacter.IsOnFloor())
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
        
                animation = "falling";
            }
        }
        
        nitroCharacter.PlayAnimation(animation);
        nitroCharacter.Velocity = velocity;
    }

    public void LaunchingAnimationFinished()
    {
        consolePrinter.Print("Launching animation finished, transitioning to flying.");

        NitroState = NitroState.Flying;
    }
}
