using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotFallingState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
    }

    public override Type? ProcessOrPass(double delta)
    {
        if (pilot.OnFloor)
        {
            return typeof(PilotIdleState);
        }
        
        pilot.Velocity = new Vector2(pilot.Velocity.X, pilot.Velocity.Y + FallingForce);

        if (pilot.Velocity.Y > MaxFallingVelocity)
        {
            pilot.Velocity = new Vector2(pilot.Velocity.X, MaxFallingVelocity);
        }
        
        return null;
    }
}
