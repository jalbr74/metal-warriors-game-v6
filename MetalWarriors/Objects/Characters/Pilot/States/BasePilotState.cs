using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public abstract class BasePilotState(IPilotCharacter nitro) : State
{
    public const float MovementSpeed = 120.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
}
