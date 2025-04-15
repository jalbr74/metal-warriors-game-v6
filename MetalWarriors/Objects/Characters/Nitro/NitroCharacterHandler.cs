using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public enum NitroDirection { Right, Left }
public enum NitroState { Idle, Walking, Launching, Falling, Flying }

// Represents Nitro as a concept, and doesn't worry about the implementation details (scene/script stuff).
public interface INitroCharacter
{
    Vector2 Velocity { get; set; }
    NitroState State { get; set; }
    NitroDirection Direction { get; set; }
    string CurrentAnimation { get; }
    bool OnFloor { get; }
    
    void PlayAnimation(string animation);
}

// This class operates on the INitro interface so that it can be used with any implementation of INitro (useful for doing TDD).
public class NitroCharacterHandler(ISnesController snesController, INitroCharacter nitroCharacter, IConsolePrinter consolePrinter)
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;

    public void PhysicsProcess(double delta)
    {
        var velocity = nitroCharacter.Velocity;
        var animation = nitroCharacter.CurrentAnimation;
        
        if (snesController.IsDPadLeftPressed)
        {
            nitroCharacter.Direction = NitroDirection.Left;
            velocity.X = -MovementSpeed;
            animation = "walking";
            nitroCharacter.State = NitroState.Walking;
        }
        else if (snesController.IsDPadRightPressed)
        {
            nitroCharacter.Direction = NitroDirection.Right;
            velocity.X = MovementSpeed;
            animation = "walking";
            nitroCharacter.State = NitroState.Walking;
        }
        else
        {
            velocity.X = 0;
            animation = "idle";
            
            if (nitroCharacter.OnFloor)
            {
                nitroCharacter.State = NitroState.Idle;
            }
        }
        
        if (snesController.IsButtonBPressed)
        {
            if (nitroCharacter.OnFloor)
            {
                velocity.Y = MaxRisingVelocity;
                animation = "launching";
                nitroCharacter.State = NitroState.Launching;
            }
            else
            {
                velocity.Y -= BoostingForce;
        
                if (velocity.Y < MaxRisingVelocity)
                {
                    velocity.Y = MaxRisingVelocity;
                }

                animation = nitroCharacter.State == NitroState.Flying ? "flying" : "launching";
            }
        }
        else
        {
            if (nitroCharacter.OnFloor)
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
                nitroCharacter.State = NitroState.Falling;
            }
        }
        
        nitroCharacter.Velocity = velocity;
        nitroCharacter.PlayAnimation(animation);
    }

    public void LaunchingAnimationFinished()
    {
        consolePrinter.Print("Launching animation finished, transitioning to flying.");

        nitroCharacter.State = NitroState.Flying;
    }
}
