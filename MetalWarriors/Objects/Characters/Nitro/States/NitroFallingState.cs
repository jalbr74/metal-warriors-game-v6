using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFallingState(ISnesController controller, INitroCharacter nitro, IConsolePrinter console) : BaseNitroState(controller, nitro, console)
{
    public override void Enter(double delta)
    {
        console.Print("Entering Falling State");
        
        nitro.PlayAnimation("falling");
        nitro.PauseAnimation();
    }
    
    public override bool ShouldTransitionToAnotherState(out string otherState)
    {
        if (controller.IsButtonBPressed)
        {
            otherState = "flying";
            return true;
        }
        
        if (nitro.OnFloor)
        {
            if (controller.IsDPadLeftPressed || controller.IsDPadRightPressed)
            {
                otherState = "walking";
                return true;
            }

            otherState = "idle";
            return true;
        }

        otherState = null;
        return false;
    }
    
    public override void PhysicsProcess(double delta)
    {
        // Nitro should be able to steer when falling
        if (controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.Left;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
        }
        else if (controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.Right;
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
    }
}
