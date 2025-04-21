using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFlyingState(ISnesController controller, INitroCharacter nitro, IConsolePrinter console) : BaseNitroState(controller, nitro, console)
{
    public override void Enter(double delta)
    {
        console.Print("Entering Flying State");
        
        nitro.PlayAnimation("flying");
        nitro.PauseAnimation();
    }
    
    public override void HandleState(double delta)
    {
        if (!nitro.OnFloor)
        {
            if (controller.IsButtonBPressed)
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
            
                if (nitro.Velocity.Y < MaxRisingVelocity)
                {
                    nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                }
            }
            else
            {
                StateMachine.TransitionTo("falling", delta);
                return;
            }
        }

        return;
        
        string animation;
        
        if (controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.Left;
            nitro.Velocity = new Vector2(-MovementSpeed, nitro.Velocity.Y);
            animation = "walking";
            nitro.State = NitroState.Walking;
        }
        else if (controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.Right;
            nitro.Velocity = new Vector2(MovementSpeed, nitro.Velocity.Y);
            animation = "walking";
            nitro.State = NitroState.Walking;
        }
        else
        {
            nitro.Velocity = new Vector2(0, nitro.Velocity.Y);
            animation = "idle";
            
            if (nitro.OnFloor)
            {
                nitro.State = NitroState.Idle;
            }
        }
        
        if (controller.IsButtonBPressed)
        {
            if (nitro.OnFloor)
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                animation = "launching";
                nitro.State = NitroState.Launching;
            }
            else
            {
                nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y - BoostingForce);
                
                if (nitro.Velocity.Y < MaxRisingVelocity)
                {
                    nitro.Velocity = new Vector2(nitro.Velocity.X, MaxRisingVelocity);
                }

                animation = nitro.State == NitroState.Flying ? "flying" : "launching";
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
        
                animation = "falling";
                nitro.State = NitroState.Falling;
            }
        }
        
        // nitro.PlayAnimation(animation);
    }
}
