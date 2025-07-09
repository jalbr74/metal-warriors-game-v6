#nullable enable
using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroIdleState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public static Vector2 AnimationOffset = Vector2.Zero;
    public static Vector2 GunOffset = new (5, -8);
    
    public override void Enter()
    {
        nitro.PlayAnimation("idle");
        nitro.PauseAnimation();
        
        nitro.AnimationOffset = AnimationOffset;
        nitro.GunOffset = GunOffset + AnimationOffset;
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        // Check if processing should be delegated to another state
        if (nitro.Controller.IsDPadLeftPressed || nitro.Controller.IsDPadRightPressed) return typeof(NitroWalkingState);
        if (nitro.Controller.IsButtonBPressed) return typeof(NitroLaunchingState);
        if (nitro.Controller.WasSelectPressed) return typeof(NitroPoweringDownState);
        if (!nitro.OnFloor) return typeof(NitroFallingState);
        
        // TODO: We should lerp to this
        nitro.Velocity = Vector2.Zero;
        
        return null;
    }
}
