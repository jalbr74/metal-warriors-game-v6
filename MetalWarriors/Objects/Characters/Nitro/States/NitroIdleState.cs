using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroIdleState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = Vector2.Zero;
    public static Vector2 GunOffset = new (5, -8);
    
    public override void Enter()
    {
        nitro.Console.Print("Entering Idle State");
        
        nitro.PlayAnimation("idle");
        nitro.PauseAnimation();
        
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffset + AnimationOffset;
    }
    
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (nitro.Controller.IsDPadLeftPressed || nitro.Controller.IsDPadRightPressed)
        {
            otherState = typeof(NitroWalkingState);
            return true;
        }
        
        if (nitro.Controller.IsButtonBPressed)
        {
            otherState = nitro.OnFloor ? typeof(NitroLaunchingState) : typeof(NitroFallingState);
            return true;
        }
        
        if (!nitro.OnFloor)
        {
            otherState = typeof(NitroFallingState);
            return true;
        }

        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        nitro.Velocity = Vector2.Zero;
        
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
        
        // nitro.PlayAnimation(animation);
        
       // return base.HandleState(delta);
    }
}
