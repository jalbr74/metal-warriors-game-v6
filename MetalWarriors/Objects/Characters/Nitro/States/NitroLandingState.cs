using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroLandingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.Console.Print("Entering Landing State");
        
        nitro.PlayAnimation("landing");
    }
    
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (nitro.IsAnimationFinished)
        {
            otherState = typeof(NitroIdleState);
            return true;
        }
        
        // if (nitro.Controller.IsButtonBPressed)
        // {
        //     otherState = "launching";
        //     return true;
        // }
        //
        // if (nitro.OnFloor)
        // {
        //     if (nitro.Controller.IsDPadLeftPressed || nitro.Controller.IsDPadRightPressed)
        //     {
        //         otherState = "walking";
        //         return true;
        //     }
        //
        //     otherState = "idle";
        //     return true;
        // }

        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        // Nitro should be able to steer when falling
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
        
        nitro.Velocity = new Vector2(nitro.Velocity.X, 0);
    }
}
