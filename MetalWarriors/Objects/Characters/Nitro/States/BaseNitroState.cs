using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class BaseNitroState(ISnesController controller, INitroCharacter nitro, IConsolePrinter console) : State
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
    
    protected void HandleGravity()
    {
        // if (nitro.OnFloor)
        // {
        //     nitro.Velocity = new Vector2(nitro.Velocity.X, 0);
        // }
        // else
        // {
        //     nitro.Velocity = new Vector2(nitro.Velocity.X, nitro.Velocity.Y + FallingForce);
        //
        //     if (nitro.Velocity.Y > MaxFallingVelocity)
        //     {
        //         nitro.Velocity = new Vector2(nitro.Velocity.X, MaxFallingVelocity);
        //     }
        //
        //     nitro.State = NitroState.Falling;
        // }
    }
}
