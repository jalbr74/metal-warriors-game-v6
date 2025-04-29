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
    
    public override bool ShouldTransitionToAnotherState(out string otherState)
    {
        if (!nitro.OnFloor)
        {
            otherState = "falling";
            return true;
        }
        
        if (!nitro.Controller.IsDPadLeftPressed && !nitro.Controller.IsDPadRightPressed)
        {
            if (nitro.Controller.IsButtonBPressed)
            {
                otherState = nitro.OnFloor ? "launching" : "flying";
            }
            else
            {
                otherState = "idle";
            }
            
            return true;
        }

        if (nitro.Controller.IsButtonBPressed)
        {
            otherState = "launching";
            return true;
        }
        
        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        if (nitro.Controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.Left;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
            // nitro.State = NitroState.Walking;
        }
        else
        {
            nitro.Direction = NitroDirection.Right;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
            // nitro.State = NitroState.Walking;
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
    
            // nitro.State = NitroState.Falling;
        }
        
        // nitro.PlayAnimation(animation);
    }
}
