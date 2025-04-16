using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class BaseNitroState(ISnesController controller, INitroCharacter nitro, IConsolePrinter console) : State
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
    
    public override string HandleState(double delta)
    {
        var velocity = nitro.Velocity;
        var animation = nitro.CurrentAnimation;
        
        if (controller.IsDPadLeftPressed)
        {
            nitro.Direction = NitroDirection.Left;
            velocity.X = -MovementSpeed;
            animation = "walking";
            nitro.State = NitroState.Walking;
        }
        else if (controller.IsDPadRightPressed)
        {
            nitro.Direction = NitroDirection.Right;
            velocity.X = MovementSpeed;
            animation = "walking";
            nitro.State = NitroState.Walking;
        }
        else
        {
            velocity.X = 0;
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
                velocity.Y = MaxRisingVelocity;
                animation = "launching";
                nitro.State = NitroState.Launching;
            }
            else
            {
                velocity.Y -= BoostingForce;
        
                if (velocity.Y < MaxRisingVelocity)
                {
                    velocity.Y = MaxRisingVelocity;
                }

                animation = nitro.State == NitroState.Flying ? "flying" : "launching";
            }
        }
        else
        {
            if (nitro.OnFloor)
            {
                velocity.Y = 0;
            }
            else
            {
                velocity.Y += FallingForce;
        
                if (velocity.Y > MaxFallingVelocity)
                {
                    velocity.Y = MaxFallingVelocity;
                }
        
                animation = "falling";
                nitro.State = NitroState.Falling;
            }
        }
        
        nitro.Velocity = velocity;
        nitro.PlayAnimation(animation);

        return null;
    }
}
