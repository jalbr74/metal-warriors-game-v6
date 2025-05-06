using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFallingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = Vector2.Zero;
    public static Vector2 GunPosition = new (11, -8);
    
    public override void Enter()
    {
        nitro.Console.Print("Entering Falling State");
        
        nitro.PlayAnimation("falling");
        nitro.CurrentAnimationOffset = AnimationOffset;
        nitro.GunPosition = GunPosition + AnimationOffset;
    }
    
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (nitro.Controller.IsButtonBPressed)
        {
            otherState = typeof(NitroFlyingState);
            return true;
        }
        
        if (nitro.OnFloor)
        {
            otherState = typeof(NitroLandingState);
            return true;
        }

        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        // Nitro should be able to steer when falling
        if (nitro.Controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.FacingLeft;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
        }
        else if (nitro.Controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.FacingRight;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
        }
        else
        {
            nitro.Velocity = new Vector2(0, nitro.Velocity.Y);
        }
        
        nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y + FallingForce);

        if (nitro.Velocity.Y > MaxFallingVelocity)
        {
            nitro.Velocity = new Vector2(nitro.Velocity.X, MaxFallingVelocity);
        }
    }
}
