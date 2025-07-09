using System;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public abstract class BasePilotState(IPilotCharacter pilot) : IState
{
    public const float MovementSpeed = 60.0f;
    public const float MaxFallingVelocity = 300.0f;
    public const float MaxRisingVelocity = -135.0f;
    public const float FallingForce = 10.0f;
    public const float BoostingForce = 10.0f;
    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual Type ProcessOrPass(double delta) { return null; }
}
