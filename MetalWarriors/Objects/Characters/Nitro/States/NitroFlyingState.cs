﻿using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFlyingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = new (-10, 4);
    public static Vector2 GunOffset = new (20, -10);
    
    public override void Enter()
    {
        nitro.Console.Print("Entering Flying State");
        
        nitro.PlayAnimation("flying");
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffset + AnimationOffset;
    }
    
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (!nitro.Controller.IsButtonBPressed)
        {
            if (!nitro.OnFloor)
            {
                otherState = typeof(NitroFallingState);
            }
            else
            {
                otherState = nitro.Controller.IsDPadLeftPressed || nitro.Controller.IsDPadRightPressed ? typeof(NitroWalkingState) : typeof(NitroIdleState);
            }
            
            return true;
        }

        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
            
        if (nitro.Velocity.Y < MaxRisingVelocity)
        {
            nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
        }
        
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

        
        // if (nitro.Controller.IsButtonBPressed)
        // {
        //     if (nitro.OnFloor)
        //     {
        //         nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
        //         // nitro.State = NitroState.Launching;
        //     }
        //     else
        //     {
        //         nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
        //         
        //         if (nitro.Velocity.Y < MaxRisingVelocity)
        //         {
        //             nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
        //         }
        //     }
        // }
        // else
        // {
        //     if (nitro.OnFloor)
        //     {
        //         nitro.Velocity = new Vector2(nitro.Velocity.X, 0);
        //     }
        //     else
        //     {
        //         nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y + FallingForce);
        //
        //         if (nitro.Velocity.Y > MaxFallingVelocity)
        //         {
        //             nitro.Velocity = new Vector2(nitro.Velocity.X, MaxFallingVelocity);
        //         }
        //
        //         // nitro.State = NitroState.Falling;
        //     }
        // }
        
        // nitro.PlayAnimation(animation);
    }
}
