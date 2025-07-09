using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroLandingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = new (-4, 0);
    public static Vector2 GunOffset = new (13, -7);
    
    public override void Enter()
    {
        nitro.PlayAnimation("landing");
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffset + AnimationOffset;
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        // Check if processing should be delegated to another state
        if (nitro.IsAnimationFinished) return typeof(NitroIdleState);
        if (nitro.Controller.IsButtonBPressed) return typeof(NitroLaunchingState);
        if (!nitro.OnFloor) return typeof(NitroFallingState);
        
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
        
        nitro.Velocity = new Vector2(nitro.Velocity.X, 0);
        
        return null;
    }
}
