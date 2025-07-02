using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroWalkingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = Vector2.Zero;
    
    public static Vector2 GunOffsetAtFrame0 = new (4, -8);
    public static Vector2 GunOffsetAtFrame1 = new (4, -9);
    public static Vector2 GunOffsetAtFrame2 = new (5, -10);
    public static Vector2 GunOffsetAtFrame3 = new (6, -8);
    public static Vector2 GunOffsetAtFrame4 = new (5, -8);
    public static Vector2 GunOffsetAtFrame5 = new (4, -9);
    public static Vector2 GunOffsetAtFrame6 = new (4, -10);
    public static Vector2 GunOffsetAtFrame7 = new (3, -7);
    
    public override void Enter()
    {
        nitro.Console.Print("Entering Walking State");
        
        nitro.PlayAnimation("walking");
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffsetAtFrame0 + AnimationOffset;
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        // Check if processing should be delegated to another state
        if (!nitro.OnFloor) return typeof(NitroFallingState);
        if (nitro.Controller.IsButtonBPressed) return typeof(NitroLaunchingState);
        if (!nitro.Controller.IsDPadLeftPressed && !nitro.Controller.IsDPadRightPressed)
        {
            if (nitro.Controller.IsButtonBPressed)
            {
                return nitro.OnFloor ? typeof(NitroLaunchingState) : typeof(NitroFlyingState);
            }
            
            return typeof(NitroIdleState);
        }
        
        if (nitro.Controller.IsDPadLeftPressed)
        {
            nitro.Direction = CharacterDirection.FacingLeft;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
        }
        else
        {
            nitro.Direction = CharacterDirection.FacingRight;
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

        nitro.GunOffset = nitro.CurrentAnimationFrame switch
        {
            0 => GunOffsetAtFrame0,
            1 => GunOffsetAtFrame1,
            2 => GunOffsetAtFrame2,
            3 => GunOffsetAtFrame3,
            4 => GunOffsetAtFrame4,
            5 => GunOffsetAtFrame5,
            6 => GunOffsetAtFrame6,
            7 => GunOffsetAtFrame7,
            _ => nitro.GunOffset
        };
        
        return null;
    }
}
