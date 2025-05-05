using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public abstract class BaseNitroState(INitroCharacter nitro) : State
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
    
    public static Vector2 GunPositionAtFrame0 = new Vector2(4, -8);
    public static Vector2 GunPositionAtFrame1 = new Vector2(4, -9);
    public static Vector2 GunPositionAtFrame2 = new Vector2(5, -10);
    public static Vector2 GunPositionAtFrame3 = new Vector2(6, -8);
    public static Vector2 GunPositionAtFrame4 = new Vector2(5, -8);
    public static Vector2 GunPositionAtFrame5 = new Vector2(4, -9);
    public static Vector2 GunPositionAtFrame6 = new Vector2(4, -10);
    public static Vector2 GunPositionAtFrame7 = new Vector2(3, -7);
    
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
