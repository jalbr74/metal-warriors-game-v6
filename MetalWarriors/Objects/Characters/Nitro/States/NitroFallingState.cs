using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFallingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = Vector2.Zero;
    public static Vector2 GunOffset = new (11, -8);
    
    public override void Enter()
    {
        nitro.Console.Print("Entering Falling State");
        
        nitro.PlayAnimation("falling");
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffset + AnimationOffset;
    }

    public override Type? ProcessOrPass(double delta)
    {
        // Check if processing should be delegated to another state
        if (nitro.Controller.IsButtonBPressed) return typeof(NitroFlyingState);
        if (nitro.OnFloor) return typeof(NitroLandingState);
        
        // Nitro should be able to steer when falling
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
        
        nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y + FallingForce);

        if (nitro.Velocity.Y > MaxFallingVelocity)
        {
            nitro.Velocity = new Vector2(nitro.Velocity.X, MaxFallingVelocity);
        }

        return null;
    }
}
