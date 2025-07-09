using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFlyingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = new (-10, 4);
    public static Vector2 GunOffset = new (20, -10);
    
    public override void Enter()
    {
        nitro.PlayAnimation("flying");
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffset + AnimationOffset;
    }

    public override Type? ProcessOrPass(double delta)
    {
        // Check if processing should be delegated to another state
        if (!nitro.Controller.IsButtonBPressed)
        {
            if (!nitro.OnFloor)
            {
                return typeof(NitroFallingState);
            }
            
            return nitro.Controller.IsDPadLeftPressed || nitro.Controller.IsDPadRightPressed ? typeof(NitroWalkingState) : typeof(NitroIdleState);
        }
        
        nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
            
        if (nitro.Velocity.Y < MaxRisingVelocity)
        {
            nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
        }
        
        if (nitro.Controller.IsDPadLeftPressed)
        {
            nitro.Direction = CharacterDirection.FacingLeft;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
        }
        else if (nitro.Controller.IsDPadRightPressed)
        {
            nitro.Direction = CharacterDirection.FacingRight;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
        }
        else
        {
            nitro.Velocity = new Vector2(0, nitro.Velocity.Y);
        }

        return null;
    }
}
