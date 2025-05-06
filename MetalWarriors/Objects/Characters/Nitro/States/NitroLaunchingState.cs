using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroLaunchingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = new (-8, 5);
    public static Vector2 GunPosition = new (19, -10);
    
    public override void Enter()
    {
        nitro.Console.Print("Entering Launching State");
        
        nitro.PlayAnimation("launching");
        nitro.CurrentAnimationOffset = AnimationOffset;
        nitro.GunPosition = GunPosition + AnimationOffset;
    }
    
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (nitro.IsAnimationFinished)
        {
            otherState = typeof(NitroFlyingState);
            return true;
        }

        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        if (nitro.Controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.FacingLeft;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
            // nitro.State = NitroState.Walking;
        }
        else if (nitro.Controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.FacingRight;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
            // nitro.State = NitroState.Walking;
        }
        else
        {
            nitro.Velocity = new Vector2(0, nitro.Velocity.Y);
            
            if (nitro.OnFloor)
            {
                // nitro.State = NitroState.Idle;
            }
        }
        
        if (nitro.Controller.IsButtonBPressed)
        {
            if (nitro.OnFloor)
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                // nitro.State = NitroState.Launching;
            }
            else
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
                
                if (nitro.Velocity.Y < MaxRisingVelocity)
                {
                    nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                }
            }
        }
        else
        {
            if (nitro.OnFloor)
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, 0);
            }
            else
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y + FallingForce);
        
                if (nitro.Velocity.Y > MaxFallingVelocity)
                {
                    nitro.Velocity = new Vector2(nitro.Velocity.X, MaxFallingVelocity);
                }
        
                // nitro.State = NitroState.Falling;
            }
        }
        
        // nitro.PlayAnimation("launching");
    }
}
