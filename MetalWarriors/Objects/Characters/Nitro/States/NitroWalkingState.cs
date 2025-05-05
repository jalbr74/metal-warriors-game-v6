using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroWalkingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.Console.Print("Entering Walking State");
        
        nitro.PlayAnimation("walking");
    }
    
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (!nitro.OnFloor)
        {
            otherState = typeof(NitroFallingState);
            return true;
        }
        
        if (!nitro.Controller.IsDPadLeftPressed && !nitro.Controller.IsDPadRightPressed)
        {
            if (nitro.Controller.IsButtonBPressed)
            {
                otherState = nitro.OnFloor ? typeof(NitroLaunchingState) : typeof(NitroFlyingState);
            }
            else
            {
                otherState = typeof(NitroIdleState);
            }
            
            return true;
        }

        if (nitro.Controller.IsButtonBPressed)
        {
            otherState = typeof(NitroLaunchingState);
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
        }
        else
        {
            nitro.Direction = NitroDirection.FacingRight;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
        }
        
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
        }

        nitro.GunPosition = nitro.CurrentAnimationFrame switch
        {
            0 => GunPositionAtFrame0,
            1 => GunPositionAtFrame1,
            2 => GunPositionAtFrame2,
            3 => GunPositionAtFrame3,
            4 => GunPositionAtFrame4,
            5 => GunPositionAtFrame5,
            6 => GunPositionAtFrame6,
            7 => GunPositionAtFrame7,
            _ => nitro.GunPosition
        };
    }
}
