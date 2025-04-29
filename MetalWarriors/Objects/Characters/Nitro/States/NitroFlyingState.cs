using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFlyingState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.Console.Print("Entering Flying State");
        
        nitro.PlayAnimation("flying");
    }
    
    public override bool ShouldTransitionToAnotherState(out string otherState)
    {
        if (!nitro.Controller.IsButtonBPressed)
        {
            if (!nitro.OnFloor)
            {
                otherState = "falling";
            }
            else
            {
                otherState = nitro.Controller.IsDPadLeftPressed || nitro.Controller.IsDPadRightPressed ? "walking" : "idle";
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
            nitro.Direction = NitroDirection.Left;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
        }
        else if (nitro.Controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.Right;
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
