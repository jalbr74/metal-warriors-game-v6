using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroWalkingState(ISnesController controller, INitroCharacter nitro, IConsolePrinter console) : BaseNitroState(controller, nitro, console)
{
    public override void Enter(double delta)
    {
        console.Print("Entering Walking State");
        
        nitro.PlayAnimation("walking");
    }
    
    public override void HandleState(double delta)
    {
        if (controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.Left;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
            // nitro.State = NitroState.Walking;
        }
        else if (controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.Right;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
            // nitro.State = NitroState.Walking;
        }
        else
        {
            StateMachine.TransitionTo("idle", delta);
            return;
            
            nitro.Velocity = new Vector2(0, nitro.Velocity.Y);
            
            if (nitro.OnFloor)
            {
                // nitro.State = NitroState.Idle;
            }
        }
        
        if (controller.IsButtonBPressed)
        {
            if (nitro.OnFloor)
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                // nitro.State = NitroState.Launching;
            }
            else
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
                
                if (nitro.Velocity.Y < MaxRisingVelocity)
                {
                    nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                }
            }
        }
        else
        {
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
        }
        
        // nitro.PlayAnimation(animation);
    }
}
