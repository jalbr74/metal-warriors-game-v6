using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroWalkingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 GunPositionAtFrame0 = new (4, -8);
    public static Vector2 GunPositionAtFrame1 = new (4, -9);
    public static Vector2 GunPositionAtFrame2 = new (5, -10);
    public static Vector2 GunPositionAtFrame3 = new (6, -8);
    public static Vector2 GunPositionAtFrame4 = new (5, -8);
    public static Vector2 GunPositionAtFrame5 = new (4, -9);
    public static Vector2 GunPositionAtFrame6 = new (4, -10);
    public static Vector2 GunPositionAtFrame7 = new (3, -7);
    
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
